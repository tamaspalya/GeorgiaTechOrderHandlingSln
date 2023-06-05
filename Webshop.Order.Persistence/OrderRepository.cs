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

        public async Task<int> CreateAsync(Domain.AggregateRoots.Order entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $@"insert into [{TableName}] (CustomerId, OrderDate, TotalPrice, OrderStatus, SellerId, DiscountId) 
                            output inserted.id
                            values (@customerId, @orderDate, @totalPrice, @orderStatus, @sellerId, @discountId)";
                var id = await connection.ExecuteScalarAsync<int>(command, new { customerId = entity.CustomerId, orderDate = entity.OrderDate, totalPrice = entity.TotalPrice, orderStatus = entity.OrderStatus, sellerId = entity.SellerId, discountId = entity.DiscountId });

                // Insert the line items after the order is created
                foreach (var lineItem in entity.OrderLineItems)
                {
                    string lineItemCommand = $@"insert into [OrderLineItem] (OrderId, ProductId, Quantity) 
                            values (@orderId, @productId, @quantity)";
                    await connection.ExecuteAsync(lineItemCommand, new { orderId = id, productId = lineItem.ProductId, quantity = lineItem.Quantity });
                }

                return id;
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"delete from [{TableName}] where id = @id";
                int rowsAffected = await connection.ExecuteAsync(command, new { id = id });
                return rowsAffected > 0; // Return true if rowsAffected is greater than 0, indicating successful deletion.
            }
        }

        public async Task<IEnumerable<Domain.AggregateRoots.Order>> GetAll()
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from [{TableName}]";
                return await connection.QueryAsync<Domain.AggregateRoots.Order>(query);
            }
        }

        public async Task<Domain.AggregateRoots.Order> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $"select * from [{TableName}] where id = @id";
                return await connection.QuerySingleAsync<Domain.AggregateRoots.Order>(query, new { id = id });
            }
        }

        public async Task UpdateAsync(Domain.AggregateRoots.Order entity)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string command = $"update [{TableName}] set CustomerId = @customerId, OrderDate = @orderDate, TotalPrice = @totalPrice, OrderStatus = @orderStatus, SellerId = @sellerId, DiscountId = @discountId where Id = @id";
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
