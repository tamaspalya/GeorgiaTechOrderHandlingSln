using AutoMapper;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Dto;
using Webshop.Order.Application.Features.GetOrders;

namespace Webshop.Order.Application.Features.GetOrder
{
    public class GetOrderQueryHandler: IQueryHandler<GetOrderQuery, OrderDto>
    {
        private ILogger<GetOrdersQueryHandler> _logger;
        private IMapper _mapper;
        private IOrderRepository _orderRepository;

        public GetOrderQueryHandler(ILogger<GetOrdersQueryHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.Order order = await _orderRepository.GetById(query.OrderId);
                return _mapper.Map<OrderDto>(order);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return Result.Fail<OrderDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
