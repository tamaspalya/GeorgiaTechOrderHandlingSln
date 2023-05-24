using Dapper;
using Webshop.Data.Persistence;
using Webshop.Order.Application.Contracts.Persistence;

namespace Webshop.Order.Persistence
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(TableNames.Order.ORDERTABLE, context)
        {
        }

        public async Task CreateAsync(Domain.AggregateRoots.Order entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"insert into {TableName} (CustomerId, OrderDate, TotalPrice, OrderStatus, SellerId, DiscountId) values (@customerId, @orderDate, @totalPrice, @orderStatus, @sellerId, @discountId)";
                await connection.ExecuteAsync(command, new { customerId = entity.CustomerId, orderDate = entity.OrderDate, totalPrice = entity.TotalPrice, orderStatus = entity.OrderStatus, sellerId = entity.SellerId, discountId = entity.DiscountId });
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"delete from {TableName} where id = @id";
                await connection.ExecuteAsync(command, new { id = id });
            }
        }

        public async Task<IEnumerable<Domain.AggregateRoots.Order>> GetAll()
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName}";
                return await connection.QueryAsync<Domain.AggregateRoots.Order>(query);
            }
        }

        public async Task<Domain.AggregateRoots.Order> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from {TableName} where id = @id";
                return await connection.QuerySingleAsync<Domain.AggregateRoots.Order>(query, new { id = id });
            }
        }

        public async Task UpdateAsync(Domain.AggregateRoots.Order entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"update {TableName} set CustomerId = @customerId, OrderDate = @orderDate, TotalPrice = @totalPrice, OrderStatus = @orderStatus, SellerId = @sellerId, DiscountId = @discountId where Id = @id";
                await connection.ExecuteAsync(command, new
                {
                    id = entity.Id,
                    customerId = entity.CustomerId,
                    orderDate = entity.OrderDate,
                    totalPrice = entity.TotalPrice,
                    orderStatus = entity.OrderStatus,
                    sellerId = entity.SellerId,
                    discountId = entity.DiscountId
                });
            }
        }
    }
}
