using Webshop.Application.Contracts.Persistence;

namespace Webshop.Order.Application.Contracts.Persistence
{
    public interface IOrderRepository: IRepository<Domain.AggregateRoots.Order>
    {
    }
}
