using AutoMapper;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Dto;

namespace Webshop.Order.Application.Features.GetOrders
{
    public class GetOrdersQueryHandler: IQueryHandler<GetOrdersQuery, List<OrderDto>>
    {
        private ILogger<GetOrdersQueryHandler> _logger;
        private IMapper _mapper;
        private IOrderRepository _orderRepository;

        public GetOrdersQueryHandler(ILogger<GetOrdersQueryHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await _orderRepository.GetAll();
                List<OrderDto> result = new List<OrderDto>();
                foreach (var element in queryResult)
                {
                    result.Add(_mapper.Map<OrderDto>(element));
                }
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<OrderDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
