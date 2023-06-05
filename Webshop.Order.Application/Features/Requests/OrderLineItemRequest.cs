using FluentValidation;
using Webshop.Domain.Common;

namespace Webshop.Order.Application.Features.Requests
{
    public class OrderLineItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public class OrderLineItemRequestValidator : AbstractValidator<OrderLineItemRequest>
        {
            public OrderLineItemRequestValidator()
            {
                RuleFor(r => r.ProductId).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(ProductId)).Code);
                RuleFor(r => r.Quantity).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Quantity)).Code);
            }
        }
    }
}
