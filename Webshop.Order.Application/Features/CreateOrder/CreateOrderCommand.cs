using EnsureThat;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application.Features.CreateOrder
{
    public class CreateOrderCommand: ICommand
    {
        public CreateOrderCommand(Domain.AggregateRoots.Order order)
        {
            Ensure.That(order, nameof(order)).IsNotNull();
            Order = order;
        }

        public Domain.AggregateRoots.Order Order { get; private set; }
    }
}
