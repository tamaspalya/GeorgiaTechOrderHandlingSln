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
using FluentValidation;
using Webshop.Order.Api.Exceptions;
using FluentValidation.Results;

namespace Webshop.Order.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : BaseController
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
            try
            {
                ValidateRequest(request);
                await ValidateCustomers(request);
                await ValidateAndFetchProducts(request);
                return await CreateOrderAsync(request);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
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
                Result createResult = await _dispatcher.Dispatch(command);
                return Ok(createResult);
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

        private void ValidateRequest(CreateOrderRequest request)
        {
            CreateOrderRequest.Validator validator = new CreateOrderRequest.Validator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

        private async Task ValidateCustomers(CreateOrderRequest request)
        {
            // Fetch and validate the buyer
            await ValidateCustomer(request.CustomerId, CustomerRoles.Buyer);

            // Fetch and validate the seller
            await ValidateCustomer(request.SellerId, CustomerRoles.Seller);
        }

        private async Task ValidateCustomer(int customerId, string role)
        {
            var result = await FetchAndValidateCustomerResult(customerId, role);

            if (!string.IsNullOrEmpty(result))
            {
                throw new CustomerException(result);
            }
        }

        private async Task ValidateAndFetchProducts(CreateOrderRequest request)
        {
            foreach (var orderLineItemRequest in request.OrderLineItems)
            {
                OrderLineItem orderLineItem = _mapper.Map<OrderLineItem>(orderLineItemRequest);
                var productResult = await ValidateAndFetchProduct(orderLineItem);
                UpdateOrderPrice(request, orderLineItem, productResult);
            }
        }

        private void UpdateOrderPrice(CreateOrderRequest request, OrderLineItem lineItem, Result<ProductDto> productResult)
        {
            _logger.LogInformation("Adding price of products to order total...");
            request.TotalPrice += lineItem.Quantity * productResult.Value.Price;
        }

        private async Task<Result<ProductDto>> ValidateAndFetchProduct(OrderLineItem lineItem)
        {
            GetProductQuery getProductQuery = new GetProductQuery(lineItem.ProductId);
            var productResult = await _dispatcher.Dispatch(getProductQuery);

            if (productResult.Failure)
            {
                throw new ProductNotFoundException(lineItem.ProductId);
            }

            if (productResult.Value.AmountInStock < lineItem.Quantity)
            {
                throw new InsufficientStockException(lineItem.ProductId, lineItem.Quantity, productResult.Value.AmountInStock);
            }

            if (!await UpdateProductStock(productResult.Value, lineItem.Quantity))
            {
                throw new StockUpdateException();
            }
            return productResult;
        }

        private async Task<IActionResult> CreateOrderAsync(CreateOrderRequest request)
        {
            Domain.AggregateRoots.Order order = _mapper.Map<Domain.AggregateRoots.Order>(request);
            CreateOrderCommand command = new CreateOrderCommand(order);
            Result createResult = await _dispatcher.Dispatch(command);

            return Ok(createResult);
        }

        private IActionResult HandleException(Exception ex)
        {
            _logger.LogError(ex.Message);
            return Error(ex.Message);
        }
        #endregion

    }
}
