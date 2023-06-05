using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Features.GetOrders;
using Webshop.Order.Application.Features.Dto;

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
    }
}
