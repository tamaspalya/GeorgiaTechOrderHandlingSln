using Microsoft.Extensions.Logging;
using Moq;
using Webshop.Service.CatalogClient;
using Webshop.Service.CatalogClient.Models;

namespace Webshop.Service.Test.Integration
{
    public class CatalogApiClientTests
    {
        private readonly ICatalogApiClient _client;
        private readonly HttpClientService _httpClientService;
        private readonly string _baseUrl = "http://localhost:8084/api";
        private readonly CatalogApiClientOptions _options;

        private ProductDto _testProduct;

        public CatalogApiClientTests()
        {
            _options = new CatalogApiClientOptions();
            _options.BaseUrl = _baseUrl;
            var logger = new Mock<ILogger<CatalogApiClient>>();
            _httpClientService = new HttpClientService(new HttpClient());
            _client = new CatalogApiClient(_httpClientService, logger.Object, _options);
        }

        private void Setup()
        {
            _testProduct = new ProductDto { Id = 1, Name = "Long Earth", SKU = "913udie193", Price = 15, Currency = "PLN", AmountInStock = 0, MinStock = 0, Description = null };
        }

        private void Teardown()
        {
            _testProduct = null;
        }

        [Fact]
        public async Task GetProduct_ReturnsProductDto_WhenProductExists()
        {
            Setup();
            // Arrange


            // Act
            var response = await _client.GetProduct(_testProduct.Id);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(_testProduct.Name, response.Product.Name);
            Assert.Equal(_testProduct.SKU, response.Product.SKU);

            Teardown();
        }

        [Fact]
        public async Task GetProduct_ReturnsNullResult_WhenProductDoesNotExist()
        {
            Setup();

            // Arrange
            int id = GenerateRandomId();

            //Act
            var response = await _client.GetProduct(id);

            //Assert
            Assert.Null(response.Product);
            Teardown();
        }

        [Fact]
        public async Task UpdateProduct_ReturnsTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var productToUpdate = new ProductDto { Id = 3, Name = "ajansifaj", SKU = "123", Price = 100, Currency = "USD", AmountInStock = 10, MinStock = 5, Description = string.Empty };

            // Act
            var result = await _client.UpdateProduct(productToUpdate);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task UpdateProduct_ReturnsFalse_WhenUpdateFails()
        {
            // Arrange
            var productToUpdate = new ProductDto { Id = 9428, Name = "Test", SKU = "123", Price = 100, Currency = "USD" };

            // Act
            var result = await _client.UpdateProduct(productToUpdate);

            // Assert
            Assert.False(result);
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
