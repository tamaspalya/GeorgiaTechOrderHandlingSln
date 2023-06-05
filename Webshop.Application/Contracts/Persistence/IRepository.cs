using Webshop.Domain.Common;

namespace Webshop.Application.Contracts.Persistence
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task<int> CreateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task UpdateAsync(T entity);
    }
}
