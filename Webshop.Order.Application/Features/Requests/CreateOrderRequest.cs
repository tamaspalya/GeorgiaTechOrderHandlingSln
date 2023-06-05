﻿using FluentValidation;
using Webshop.Domain.Common;

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

        public class Validator : AbstractValidator<CreateOrderRequest>
        {
            public Validator()
            {
                RuleFor(r => r.CustomerId).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                RuleFor(r => r.OrderDate).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                RuleFor(r => r.TotalPrice).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                RuleFor(r => r.OrderStatus).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                RuleFor(r => r.SellerId).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
                RuleFor(r => r.DiscountId).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code);
            }
        }
    }
}
