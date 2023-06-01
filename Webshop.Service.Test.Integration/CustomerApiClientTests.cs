using Microsoft.Extensions.Logging;
using Moq;
using Webshop.Service.CatalogClient;
using Webshop.Service.CatalogClient.Models;
using Webshop.Service.CustomerClient;
using Webshop.Service.CustomerClient.Models;

namespace Webshop.Service.Test.Integration
{
    public class CustomerApiClientTests
    {
        private readonly ICustomerApiClient _client;
        private readonly HttpClientService _httpClientService;
        private readonly string _baseUrl = "http://localhost:8085/api";

        private CustomerDto _testCustomer;

        public CustomerApiClientTests()
        {
            var logger = new Mock<ILogger<CustomerApiClient>>();
            _httpClientService = new HttpClientService(new HttpClient());
            _client = new CustomerApiClient(_httpClientService, logger.Object, _baseUrl);
        }

        private void Setup()
        {
            _testCustomer = new CustomerDto 
            {
                Id = 1,
                Name = "Tamas",
                Address = string.Empty,
                Address2 = string.Empty,
                City = string.Empty,
                Region = string.Empty,
                PostalCode = string.Empty,
                Country = string.Empty,
                Email = "tamas@email.dk"
            };
        }

        private void Teardown()
        {
            _testCustomer = null;
        }

        [Fact]
        public async Task GetProduct_ReturnsProductDto_WhenProductExists()
        {
            Setup();
            // Arrange


            // Act
            var response = await _client.GetCustomer(_testCustomer.Id);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(_testCustomer.Id, response.Customer.Id);
            Assert.Equal(_testCustomer.Name, response.Customer.Name);

            Teardown();
        }

        [Fact]
        public async Task GetProduct_ReturnsNullResult_WhenProductDoesNotExist()
        {
            Setup();

            // Arrange
            int id = GenerateRandomId();

            //Act
            var response = await _client.GetCustomer(id);

            //Assert
            Assert.Null(response.Customer);
            Teardown();
        }

        #region Helper Methods
        private int GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(1000, 10000);
        }
        #endregion
    }

}
