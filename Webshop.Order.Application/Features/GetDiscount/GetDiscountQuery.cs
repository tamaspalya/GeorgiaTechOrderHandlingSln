using EnsureThat;
using Webshop.Application.Contracts;
using Webshop.Order.Application.Features.Dto;

namespace Webshop.Order.Application.Features.GetDiscount
{
    public class GetDiscountQuery: IQuery<DiscountDto>
    {
        public GetDiscountQuery(int discountId)
        {
            DiscountId = discountId;
        }

        public int DiscountId { get; private set; }
    }
}
