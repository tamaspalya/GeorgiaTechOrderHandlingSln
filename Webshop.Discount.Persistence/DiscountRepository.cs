using Webshop.Data.Persistence;
using Webshop.Domain.AggregateRoots;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Discount.Persistence
{
    public class DiscountRepository : BaseRepository, IDiscountRepository
    {
        public DiscountRepository(string tableName, DataContext dataContext) : base(tableName, dataContext)
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

        public Task<Discount> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $@"SELECT * FROM [{TableName}] WHERE Id = @id;";

                using (var multi = await connection.QueryMultipleAsync(query, new { id = id }))
                {
                    var order = multi.Read<Domain.AggregateRoots.Order>().SingleOrDefault();
                    var orderLineItems = multi.Read<Domain.AggregateRoots.OrderLineItem>().ToList();

                    if (order != null)
                    {
                        order.OrderLineItems = orderLineItems;
                    }

                    return order;
                }
            }
        }

        public Task UpdateAsync(Discount entity)
        {
            throw new NotImplementedException();
        }
    }
}
