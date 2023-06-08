using Dapper;
using Webshop.Data.Persistence;
using Webshop.Domain.AggregateRoots;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Order.Persistence
{
    public class DiscountRepository : BaseRepository, IDiscountRepository
    {
        public DiscountRepository(DataContext context) : base(TableNames.Discount.DISCOUNTTABLE, context)
        {
        }

        public Task<int> CreateAsync(Discount entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Discount>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Discount> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where id = @id";
                return await connection.QuerySingleAsync<Discount>(query, new { id = id });
            }
        }

        public Task UpdateAsync(Discount entity)
        {
            throw new NotImplementedException();
        }
    }
}
