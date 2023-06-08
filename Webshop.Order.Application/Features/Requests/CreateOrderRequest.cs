using FluentValidation;
using Webshop.Domain.Common;
using static Webshop.Order.Application.Features.Requests.OrderLineItemRequest;

namespace Webshop.Order.Application.Features.Requests
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public int SellerId { get; set; }
        public int DiscountId { get; set; }
        public List<OrderLineItemRequest> OrderLineItems { get; set; }

        public class Validator : AbstractValidator<CreateOrderRequest>
        {
            public Validator()
            {
                RuleFor(r => r.CustomerId).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                RuleFor(r => r.OrderDate).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                RuleFor(r => r.OrderStatus).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                RuleFor(r => r.SellerId).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                //RuleFor(r => r.DiscountId).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);

                RuleFor(r => r)
                    .Must(r => r.CustomerId != r.SellerId)
                    .WithMessage("CustomerId and SellerId must not be the same");

                RuleForEach(r => r.OrderLineItems).SetValidator(new OrderLineItemRequestValidator());

                RuleFor(r => r.OrderLineItems)
                    .Must(lineItems => lineItems != null && lineItems.Any())
                    .WithMessage("OrderLineItems must not be empty");

                RuleFor(r => r.OrderLineItems)
                    .Must(lineItems => lineItems.GroupBy(i => i.ProductId).All(g => g.Count() == 1))
                    .WithMessage("There must not be multiple OrderLineItems with the same ProductId");
            }
        }
    }
}
