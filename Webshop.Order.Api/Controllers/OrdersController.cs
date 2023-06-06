using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Features.GetOrders;
using Webshop.Order.Application.Features.Dto;
using Webshop.Order.Application.Features.GetOrder;
using Webshop.Order.Application.Features.Requests;
using Webshop.Order.Application.Features.CreateOrder;
using Webshop.Order.Application.Features.DeleteOrder;
using Webshop.Order.Application.Features.UpdateOrder;
using Webshop.Order.Application.ClientFeatures.Customer;
using Webshop.Order.Application.ClientFeatures.Customer.GetCustomer;
using Webshop.Order.Api.Constants;
using Webshop.Order.Application.ClientFeatures.Catalog.GetProduct;
using Webshop.Order.Application.ClientFeatures.Catalog.Requests;
using Webshop.Domain.AggregateRoots;
using Webshop.Order.Application.ClientFeatures.Catalog;
using Webshop.Order.Application.ClientFeatures.Catalog.UpdateProduct;

namespace Webshop.Order.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController: BaseController
    {
        private IDispatcher _dispatcher;
        private IMapper _mapper;
        private ILogger<OrdersController> _logger;



        public OrdersController(IDispatcher dispatcher, IMapper mapper, ILogger<OrdersController> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            GetOrdersQuery query = new GetOrdersQuery();
            Result<List<OrderDto>> result = await _dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult(result);
            }
            else
            {
                _logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            GetOrderQuery query = new GetOrderQuery(id);
            Result<OrderDto> result = await _dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult(result);
            }
            else
            {
                _logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                //All inputs are valid, calling customer API

                //Fetch the buyer
                var buyerResult = await FetchAndValidateCustomerResult(request.CustomerId, CustomerRoles.Buyer);
                if (!string.IsNullOrEmpty(buyerResult))
                {
                    return Error(buyerResult);
                }

                //Fetch the seller
                var sellerResult = await FetchAndValidateCustomerResult(request.SellerId, CustomerRoles.Seller);
                if (!string.IsNullOrEmpty(sellerResult)) 
                {
                    return Error(sellerResult);
                }

                _logger.LogInformation("Fetching products for order...");
                // Validate and fetch each product in the order
                foreach (var orderLineItem in request.OrderLineItems)
                {
                    GetProductQuery getProductQuery = new GetProductQuery(orderLineItem.ProductId);
                    var productResult = await _dispatcher.Dispatch(getProductQuery);

                    if (productResult.Failure)
                    {
                        string errorMessage = $"Product with id {orderLineItem.ProductId} not found";
                        _logger.LogError(errorMessage);
                        return Error(errorMessage);
                    }

                    //TODO: Validate quantity
                    if (productResult.Value.AmountInStock < orderLineItem.Quantity)
                    {
                        string errorMessage = $"There are not enough products in stock for product with id: {orderLineItem.ProductId}. Requested quantity: {orderLineItem.Quantity}. Amount in stock: {productResult.Value.AmountInStock}";
                        _logger.LogError(errorMessage);
                        return Error(errorMessage);
                    }

                    if (!await UpdateProductStock(productResult.Value, orderLineItem.Quantity))
                    {
                        string errorMessage = "Stock could not be updated.";
                        _logger.LogError(errorMessage);
                        return Error(errorMessage);
                    }

                    _logger.LogInformation("Adding price of products to order total...");
                    request.TotalPrice += orderLineItem.Quantity * productResult.Value.Price;
                }



                Domain.AggregateRoots.Order order = _mapper.Map<Domain.AggregateRoots.Order>(request);
                CreateOrderCommand command = new CreateOrderCommand(order);
                Result createResult = await _dispatcher.Dispatch(command);
                return Ok(createResult);
            }
            else
            {
                _logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            DeleteOrderCommand command = new DeleteOrderCommand(id);
            Result result = await _dispatcher.Dispatch(command);
            if (result.Success)
            {
                return FromResult(result);
            }
            else
            {
                _logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            UpdateOrderRequest.Validator validator = new UpdateOrderRequest.Validator();
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                Domain.AggregateRoots.Order customer = _mapper.Map<Domain.AggregateRoots.Order>(request);
                UpdateOrderCommand command = new UpdateOrderCommand(customer);
                Result updateResult = await _dispatcher.Dispatch(command);
                return Ok(updateResult);
            }
            else
            {
                _logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }
        }

        #region External api methods
        private async Task<Result<CustomerDto>> FetchCustomer(int customerId)
        {
            var getCustomerQuery = new GetCustomerQuery(customerId);
            return await _dispatcher.Dispatch(getCustomerQuery);
        }

        private async Task<string> FetchAndValidateCustomerResult(int customerId, string role)
        {
            var customerResult = await FetchCustomer(customerId);

            if (customerResult.Failure)
            {
                string errorMessage = $"Failed to fetch {role} with id: {customerId} from Customer API.";
                _logger.LogError(errorMessage);
                return errorMessage;
            }

            return string.Empty;
        }

        private async Task<bool> UpdateProductStock(ProductDto product, int quantity)
        {
            UpdateProductRequest updateProductRequest = _mapper.Map<UpdateProductRequest>(product);

            if (updateProductRequest.AmountInStock == 1)
            {
                _logger.LogError("Minimum stock cannot be less than 1.");
                return false;
            }

            updateProductRequest.AmountInStock -= quantity;

            UpdateProductRequest.Validator validator = new UpdateProductRequest.Validator();
            var result = validator.Validate(updateProductRequest);
            if (result.IsValid)
            {
                Service.CatalogClient.Models.ProductDto productToUpdate = _mapper.Map<Service.CatalogClient.Models.ProductDto>(updateProductRequest);
                UpdateProductCommand command = new UpdateProductCommand(productToUpdate);
                Result updateResult = await _dispatcher.Dispatch(command);
                return true;
            }
            else
            {
                _logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return false;
            }
        }
        #endregion

        #region Helper methods
        private bool CheckStockAvailability(int requestedQuantity, int amountInStock)
        {
            if (amountInStock < requestedQuantity)
            {
                string errorMessage = $"Stock for product is too low. Requested quantity: {requestedQuantity}. Amount in stock: {amountInStock}";
                _logger.LogError(errorMessage);
                return false;
            }
            return true;
        }
        #endregion
    }
}
