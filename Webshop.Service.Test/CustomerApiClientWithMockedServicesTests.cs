using Moq;
using Microsoft.Extensions.Logging;
using Webshop.Service.CustomerClient;
using Webshop.Service.CustomerClient.Models.Responses;
using Webshop.Service.CustomerClient.Models;
using Webshop.Service.CustomerClient.Exceptions;

namespace Webshop.Service.Test
{
    public class CustomerApiClientWithMockedServicesTests
    {
        private readonly Mock<IHttpClientService> _httpClientServiceMock;
        private readonly Mock<ILogger<CustomerApiClient>> _loggerMock;
        private readonly string _testBaseUrl = "http://testurl.com";
        private readonly ICustomerApiClient _customerApiClient;

        public CustomerApiClientWithMockedServicesTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
            _loggerMock = new Mock<ILogger<CustomerApiClient>>();
            _customerApiClient = new CustomerApiClient(_httpClientServiceMock.Object, _loggerMock.Object, _testBaseUrl);
        }

        [Fact]
        public async Task GetCustomer_ValidId_ReturnsCustomer()
        {
            // Arrange
            var customerId = 1;
            var customerResponse = new CustomerResponse { Result = new CustomerDto { Id = customerId } };

            _httpClientServiceMock.Setup(client => client.GetAsync<CustomerResponse>(It.IsAny<string>()))
                .ReturnsAsync(customerResponse);

            // Act
            var result = await _customerApiClient.GetCustomer(customerId);

            // Assert
            Assert.Equal("Success", result.Status);
            Assert.Equal(customerId, result.Customer.Id);
        }

        [Fact]
        public async Task GetProduct_InvalidId_ThrowsException()
        {
            // Arrange
            var productId = -1;
            _httpClientServiceMock.Setup(client => client.GetAsync<CustomerResponse>(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<CustomerApiClientException>(() => _customerApiClient.GetCustomer(productId));
        }
    }
}
