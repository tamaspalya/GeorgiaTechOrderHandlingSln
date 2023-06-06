using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Webshop.Order.Application.Features.Requests.OrderLineItemRequest;
using Webshop.Domain.Common;
using Webshop.Order.Application.Features.Requests;

namespace Webshop.Order.Application.ClientFeatures.Catalog.Requests
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public string? Description { get; set; }
        public int AmountInStock { get; set; }
        public int? MinStock { get; set; }

        public class Validator : AbstractValidator<UpdateProductRequest>
        {
            public Validator()
            {
                RuleFor(r => r.Id)
                    .NotNull().WithMessage(Errors.General.ValueIsRequired(nameof(Id)).Code)
                    .GreaterThanOrEqualTo(0).WithMessage(Errors.General.ValueTooSmall(nameof(Id), 1).Code);
                RuleFor(r => r.AmountInStock)
                    .NotNull().WithMessage(Errors.General.ValueIsRequired(nameof(Id)).Code)
                    .GreaterThanOrEqualTo(0).WithMessage(Errors.General.ValueTooSmall(nameof(Id), 1).Code);
            }
        }
    }
}
