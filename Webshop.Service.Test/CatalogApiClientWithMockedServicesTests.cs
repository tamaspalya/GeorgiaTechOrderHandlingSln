using Moq;
using Microsoft.Extensions.Logging;
using Webshop.Service.CatalogClient;
using Webshop.Service.CatalogClient.Models;
using Webshop.Service.CatalogClient.Models.Responses;
using Webshop.Service.CatalogClient.Exceptions;

namespace Webshop.Service.Test
{
    public class CatalogApiClientWithMockedServicesTests
    {
        private readonly Mock<IHttpClientService> _httpClientServiceMock;
        private readonly Mock<ILogger<CatalogApiClient>> _loggerMock;
        private readonly string _testBaseUrl = "http://testurl.com";
        private readonly CatalogApiClient _catalogApiClient;

        public CatalogApiClientWithMockedServicesTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
            _loggerMock = new Mock<ILogger<CatalogApiClient>>();
            _catalogApiClient = new CatalogApiClient(_httpClientServiceMock.Object, _loggerMock.Object, _testBaseUrl);
        }

        [Fact]
        public async Task GetProduct_ValidId_ReturnsProduct()
        {
            // Arrange
            var productId = 1;
            var productResponse = new ProductResponse { Result = new ProductDto { Id = productId } };

            _httpClientServiceMock.Setup(client => client.GetAsync<ProductResponse>(It.IsAny<string>()))
                .ReturnsAsync(productResponse);

            // Act
            var result = await _catalogApiClient.GetProduct(productId);

            // Assert
            Assert.Equal("Success", result.Status);
            Assert.Equal(productId, result.Product.Id);
        }

        [Fact]
        public async Task GetProduct_InvalidId_ThrowsException()
        {
            // Arrange
            var productId = -1;
            _httpClientServiceMock.Setup(client => client.GetAsync<ProductResponse>(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<CatalogApiClientException>(() => _catalogApiClient.GetProduct(productId));
        }

        [Fact]
        public async Task UpdateProduct_ValidProduct_ReturnsTrue()
        {
            // Arrange
            var product = new ProductDto { Id = 1 };
            var productResponse = new ProductResponse { Result = product };

            _httpClientServiceMock.Setup(client => client.UpdateAsync<ProductDto, ProductResponse>(It.IsAny<string>(), product))
                .ReturnsAsync(productResponse);
            _httpClientServiceMock.Setup(client => client.GetAsync<ProductResponse>(It.IsAny<string>()))
                .ReturnsAsync(productResponse);

            // Act
            var result = await _catalogApiClient.UpdateProduct(product);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task UpdateProduct_InvalidProduct_ThrowsException()
        {
            // Arrange
            var product = new ProductDto { Id = -1 };
            _httpClientServiceMock.Setup(client => client.UpdateAsync<ProductDto, ProductResponse>(It.IsAny<string>(), product))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<CatalogApiClientException>(() => _catalogApiClient.UpdateProduct(product));
        }
    }
}
