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
                Domain.AggregateRoots.Order customer = _mapper.Map<Domain.AggregateRoots.Order>(request);
                CreateOrderCommand command = new CreateOrderCommand(customer);
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
                Result createResult = await _dispatcher.Dispatch(command);
                return Ok(createResult);
            }
            else
            {
                _logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }
        }
    }
}
