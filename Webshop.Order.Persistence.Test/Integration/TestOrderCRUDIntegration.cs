using Dapper;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Data;
using Webshop.Application.Contracts.Persistence;
using Webshop.Data.Persistence;
using Webshop.Domain.AggregateRoots;
using Webshop.Order.Application.Contracts.Persistence;
using Xunit;

namespace Webshop.Order.Persistence.Test.Integration
{
    public class TestOrderCRUDIntegration
    {
        private readonly DataContext _dataContext;
        private readonly IOrderRepository _orderRepository;

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

        [Fact]
        public async Task CreateOrder_Success()
        {
            
        }

        [Fact]
        public async Task DeleteOrder_Success()
        {
            
        }

        [Fact]
        public async Task GetAllOrders_Success()
        {
            
        }

        [Fact]
        public async Task GetOrderById_Success()
        {
            //Arrange
            Domain.AggregateRoots.Order newOrder = new Domain.AggregateRoots.Order
            {
                CustomerId = 1,
                OrderDate = RoundToSecond(DateTime.Now),
                TotalPrice = 35.5,
                OrderStatus = "Shipped",
                SellerId = 2,
                DiscountId = 3
            };

            //Act
            var retrievedId = await _orderRepository.CreateAsync(newOrder);

            //Act
            var retrievedOrder = await _orderRepository.GetById(retrievedId);

            //Assert
            Assert.NotNull(retrievedOrder);
            Assert.Equal(retrievedId, retrievedOrder.Id);
            Assert.Equal(newOrder.CustomerId, retrievedOrder.CustomerId);
            Assert.Equal(newOrder.OrderDate, retrievedOrder.OrderDate);
            Assert.Equal(newOrder.TotalPrice, retrievedOrder.TotalPrice);
            Assert.Equal(newOrder.OrderStatus, retrievedOrder.OrderStatus);
            Assert.Equal(newOrder.SellerId, retrievedOrder.SellerId);
            Assert.Equal(newOrder.DiscountId, retrievedOrder.DiscountId);
        }

        [Fact]
        public async Task UpdateOrder_Success()
        {
            
        }

        #region HelperMethods
        public DateTime RoundToSecond(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                                dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind);
        }
        #endregion

    }
}