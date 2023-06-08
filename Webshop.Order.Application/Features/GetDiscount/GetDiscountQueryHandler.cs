using AutoMapper;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Application.Features.Dto;
using Webshop.Order.Application.Features.GetOrder;

namespace Webshop.Order.Application.Features.GetDiscount
{
    public class GetDiscountQueryHandler: IQueryHandler<GetDiscountQuery, DiscountDto>
    {
        private ILogger<GetOrderQueryHandler> _logger;
        private IMapper _mapper;
        private IDiscountRepository _discountRepository;

        public GetDiscountQueryHandler(ILogger<GetOrderQueryHandler> logger, IMapper mapper, IDiscountRepository discountRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _discountRepository = discountRepository;
        }

        public async Task<Result<DiscountDto>> Handle(GetDiscountQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                Discount discount = await _discountRepository.GetById(query.DiscountId);
                return _mapper.Map<DiscountDto>(discount);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return Result.Fail<DiscountDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
