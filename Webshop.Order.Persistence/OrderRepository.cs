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
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string command = $@"insert into [{TableName}] (CustomerId, OrderDate, TotalPrice, OrderStatus, SellerId, DiscountId) 
                        output inserted.id
                        values (@customerId, @orderDate, @totalPrice, @orderStatus, @sellerId, @discountId)";
                        var id = await connection.ExecuteScalarAsync<int>(command, new { customerId = entity.CustomerId, orderDate = entity.OrderDate, totalPrice = entity.TotalPrice, orderStatus = entity.OrderStatus, sellerId = entity.SellerId, discountId = entity.DiscountId }, transaction);

                        // Insert the line items after the order is created
                        foreach (var lineItem in entity.OrderLineItems)
                        {
                            string lineItemCommand = $@"insert into [OrderLineItem] (OrderId, ProductId, Quantity) 
                        values (@orderId, @productId, @quantity)";
                            await connection.ExecuteAsync(lineItemCommand, new { orderId = id, productId = lineItem.ProductId, quantity = lineItem.Quantity }, transaction);
                        }

                        transaction.Commit();
                        return id;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw; // rethrow the exception so it can be handled upstream
                    }
                }
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
                string query = $@"SELECT * FROM [{TableName}]
                          SELECT * FROM [OrderLineItem] ORDER BY OrderId";

                using (var multi = await connection.QueryMultipleAsync(query))
                {
                    var orderDictionary = new Dictionary<int, Domain.AggregateRoots.Order>();

                    var orders = multi.Read<Domain.AggregateRoots.Order>();
                    foreach (var order in orders)
                    {
                        orderDictionary[order.Id] = order;
                    }

                    var orderLineItems = multi.Read<Domain.AggregateRoots.OrderLineItem>()
                        .GroupBy(orderLineItem => orderLineItem.OrderId);

                    foreach (var group in orderLineItems)
                    {
                        if (orderDictionary.TryGetValue(group.Key, out Domain.AggregateRoots.Order order))
                        {
                            order.OrderLineItems = group.ToList();
                        }
                    }

                    return orderDictionary.Values;
                }
            }
        }

        public async Task<Domain.AggregateRoots.Order> GetById(int id)
        {
            using (var connection = dataContext.CreateConnection())
            {
                string query = $@"SELECT * FROM [{TableName}] WHERE Id = @id;
                          SELECT * FROM [OrderLineItem] WHERE OrderId = @id;";

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
