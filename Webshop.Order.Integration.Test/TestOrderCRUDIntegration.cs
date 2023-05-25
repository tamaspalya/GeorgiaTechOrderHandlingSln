using Microsoft.Extensions.Configuration;
using Webshop.Data.Persistence;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Order.Persistence;

namespace Webshop.Order.Integration.Test
{
    public class TestOrderCRUDIntegration: IDisposable
    {
        private readonly DataContext _dataContext;
        private readonly IOrderRepository _orderRepository;

        private Domain.AggregateRoots.Order _testOrder;

        public TestOrderCRUDIntegration()
        {
            // Load configuration

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _dataContext = new DataContext(config);
            _orderRepository = new OrderRepository(_dataContext);
            
        }

        private async Task SetupAsync()
        {
            await CreateOrderAsync();
        }

        private async Task TeardownAsync()
        {
            await _orderRepository.DeleteAsync(_testOrder.Id);
        }

        [Fact]
        public async Task CreateOrder_Success()
        {
            await SetupAsync();
            //Arrange
            //  Done in setup
            //Act
            //  Order created in setup

            //Assert
            Assert.True(_testOrder.Id > 0);

            await TeardownAsync();
        }

        [Fact]
        public async Task DeleteOrder_Success()
        {
            await SetupAsync();
            //Arrange
            //  Done in setup

            //Act
            bool result = await _orderRepository.DeleteAsync(_testOrder.Id);

            //Assert
            Assert.True(result, "Failed to delete order from the database.");

            await TeardownAsync();
        }

        [Fact]
        public async Task GetAllOrders_Success()
        {
            await SetupAsync();
            //Arrange
            //Act
            var result = await _orderRepository.GetAll();
            //Assert
            Assert.NotNull(result);
            Assert.Contains(_testOrder, result);

            await TeardownAsync();
        }

        [Fact]
        public async Task GetOrderById_Success()
        {
            await SetupAsync();
            //Arrange
            // Done in setup 

            //Act
            var retrievedOrder = await _orderRepository.GetById(_testOrder.Id);

            //Assert
            Assert.NotNull(retrievedOrder);
            Assert.Equal(_testOrder.Id, retrievedOrder.Id);
            Assert.Equal(_testOrder.CustomerId, retrievedOrder.CustomerId);
            Assert.Equal(_testOrder.OrderDate, retrievedOrder.OrderDate);
            Assert.Equal(_testOrder.TotalPrice, retrievedOrder.TotalPrice);
            Assert.Equal(_testOrder.OrderStatus, retrievedOrder.OrderStatus);
            Assert.Equal(_testOrder.SellerId, retrievedOrder.SellerId);
            Assert.Equal(_testOrder.DiscountId, retrievedOrder.DiscountId);

            await TeardownAsync();
        }

        [Fact]
        public async Task UpdateOrder_Success()
        {
            await SetupAsync();

            //Arrange
            var orderWithUpdates = new Domain.AggregateRoots.Order 
            { 
                Id = _testOrder.Id,
                CustomerId = 16,
                OrderDate = RoundToSecond(DateTime.UtcNow),
                TotalPrice = 78.6,
                OrderStatus = "Delivered",
                SellerId = 20,
                DiscountId = 21,
            };

            //Act
            await _orderRepository.UpdateAsync(orderWithUpdates);

            //fetch the updated order
            var updatedOrder = await _orderRepository.GetById(_testOrder.Id);

            //Assert
            Assert.NotNull(updatedOrder);
            Assert.Equal(orderWithUpdates.Id, updatedOrder.Id);
            Assert.Equal(orderWithUpdates.CustomerId, updatedOrder.CustomerId);
            Assert.Equal(orderWithUpdates.OrderDate, updatedOrder.OrderDate);
            Assert.Equal(orderWithUpdates.TotalPrice, updatedOrder.TotalPrice);
            Assert.Equal(orderWithUpdates.OrderStatus, updatedOrder.OrderStatus);
            Assert.Equal(orderWithUpdates.SellerId, updatedOrder.SellerId);
            Assert.Equal(orderWithUpdates.DiscountId, updatedOrder.DiscountId);

            await TeardownAsync();
        }

        #region HelperMethods
        private DateTime RoundToSecond(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                                dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind);
        }

        private async Task CreateOrderAsync()
        {
            //Arrange
            _testOrder = new Domain.AggregateRoots.Order
            {
                CustomerId = 1,
                OrderDate = RoundToSecond(DateTime.Now),
                TotalPrice = 35.5,
                OrderStatus = "Shipped",
                SellerId = 2,
                DiscountId = 3
            };

            //Act
            _testOrder.Id = await _orderRepository.CreateAsync(_testOrder);
        }

        public void Dispose()
        {
            _testOrder = null;
        }
        #endregion

    }
}