using Dapper;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Data;
using Webshop.Application.Contracts.Persistence;
using Webshop.Data.Persistence;
using Webshop.Order.Application.Contracts.Persistence;
using Xunit;

namespace Webshop.Order.Persistence.Test.Unit
{
    public class TestOrderCRUDWithMockedRepository
    {
        private readonly DataContext _dataContext;
        private readonly IOrderRepository _orderRepository;

        public TestOrderCRUDWithMockedRepository()
        {
            // Load configuration
            /*
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _dataContext = new DataContext(config);
            _orderRepository = new OrderRepository(_dataContext);
            */
        }

        /*
        [Fact]
        public async Task CreateOrder_Success()
        {
            // Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();

            Domain.AggregateRoots.Order expectedOrder = new Domain.AggregateRoots.Order();

            mockOrderRepository.Setup(repo => repo.CreateAsync(It.IsAny<Domain.AggregateRoots.Order>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await mockOrderRepository.Object.CreateAsync(expectedOrder);

            // Assert
            mockOrderRepository.Verify(repo => repo.CreateAsync(It.IsAny<Domain.AggregateRoots.Order>()), Times.Once);
        }
        */

        [Fact]
        public async Task DeleteOrder_Success()
        {
            // Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var expectedOrderId = 1;

            mockOrderRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await mockOrderRepository.Object.DeleteAsync(expectedOrderId);

            // Assert
            mockOrderRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllOrders_Success()
        {
            // Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var expectedOrders = new List<Domain.AggregateRoots.Order>
            {
                new Domain.AggregateRoots.Order(),
                new Domain.AggregateRoots.Order()
            };

            mockOrderRepository.Setup(repo => repo.GetAll())
                .ReturnsAsync(expectedOrders)
                .Verifiable();

            // Act
            var result = await mockOrderRepository.Object.GetAll();

            // Assert
            mockOrderRepository.Verify(repo => repo.GetAll(), Times.Once);
            Assert.Equal(expectedOrders, result);
        }

        [Fact]
        public async Task GetOrderById_Success()
        {
            // Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var expectedOrderId = 1;
            var expectedOrder = new Domain.AggregateRoots.Order();

            mockOrderRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(expectedOrder)
                .Verifiable();

            // Act
            var result = await mockOrderRepository.Object.GetById(expectedOrderId);

            // Assert
            mockOrderRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            Assert.Equal(expectedOrder, result);
        }

        [Fact]
        public async Task UpdateOrder_Success()
        {
            // Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var orderToUpdate = new Domain.AggregateRoots.Order();

            mockOrderRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Domain.AggregateRoots.Order>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await mockOrderRepository.Object.UpdateAsync(orderToUpdate);

            // Assert
            mockOrderRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.AggregateRoots.Order>()), Times.Once);
        }

    }
}