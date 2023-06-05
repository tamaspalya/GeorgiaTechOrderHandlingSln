using EnsureThat;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application.Features.UpdateOrder
{
    public class UpdateOrderCommand: ICommand
    {
        public UpdateOrderCommand(Domain.AggregateRoots.Order order)
        {
            Ensure.That(order, nameof(order)).IsNotNull();
            Ensure.That(order.Id, nameof(order.Id)).IsNotDefault();
            Ensure.That(order.Id, nameof(order.Id)).IsGt<int>(0);
            Ensure.That(order.CustomerId, nameof(order.CustomerId)).IsNotDefault();
            Ensure.That(order.CustomerId, nameof(order.CustomerId)).IsGt<int>(0);
            Ensure.That(order.SellerId, nameof(order.SellerId)).IsNotDefault();
            Ensure.That(order.SellerId, nameof(order.SellerId)).IsGt<int>(0);

            Order = order;
        }

        public Domain.AggregateRoots.Order Order { get; private set; }
}
}
