using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Service.PaymentClient.Models.Requests;
using Webshop.Service.PaymentClient.Models.Responses;
using Webshop.Service.PaymentClient;
using Webshop.Service.PaymentClient.Constants;
using Webshop.Service.PaymentClient.Exceptions;

namespace Webshop.Service.Test
{
    public class PaymentApiClientWithMockedServicesTests
    {
        private readonly Mock<IHttpClientService> _httpClientServiceMock;
        private readonly Mock<ILogger<PaymentApiClient>> _loggerMock;
        private readonly string _baseUrl;
        private readonly PaymentApiClient _paymentApiClient;

        private readonly string _processPaymentEndpoint = "payment/process";

        public PaymentApiClientWithMockedServicesTests()
        {
            _httpClientServiceMock = new Mock<IHttpClientService>();
            _loggerMock = new Mock<ILogger<PaymentApiClient>>();
            _baseUrl = "http://test-url.com";
            _paymentApiClient = new PaymentApiClient(_httpClientServiceMock.Object, _loggerMock.Object, _baseUrl);
        }

        [Fact]
        public async Task ProcessPayment_ReturnsSuccess_WhenApiCallIsSuccessful()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                // Initialize with your values
            };

            var apiResponse = new TransactionResult
            {
                IsSuccess = true,
                Value = new Transaction { TransactionId = "123" }
            };

            _httpClientServiceMock.Setup(s => s.PostAsync<PaymentRequest, TransactionResult>(
                It.Is<string>(url => url == $"{_baseUrl}/{_processPaymentEndpoint}"), paymentRequest))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _paymentApiClient.ProcessPayment(paymentRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Error);
            Assert.Equal("123", result.TransactionId);

            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task ProcessPayment_ThrowsException_WhenApiResponseIsNull()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                // Initialize with your values
            };

            _httpClientServiceMock.Setup(s => s.PostAsync<PaymentRequest, TransactionResult>(
                It.Is<string>(url => url == $"{_baseUrl}/{_processPaymentEndpoint}"), paymentRequest))
                .ReturnsAsync((TransactionResult)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<PaymentApiClientException>(() => _paymentApiClient.ProcessPayment(paymentRequest));
            Assert.Contains("Response from API was null", ex.Message);

            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Between(1, 2, Moq.Range.Inclusive));
        }

        [Fact]
        public async Task ProcessPayment_ReturnsFailure_WhenApiCallIsUnsuccessful()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                // Initialize with your values
            };

            var apiResponse = new TransactionResult
            {
                IsSuccess = false,
                Error = "Some error"
            };

            _httpClientServiceMock.Setup(s => s.PostAsync<PaymentRequest, TransactionResult>(
                It.Is<string>(url => url == $"{_baseUrl}/{_processPaymentEndpoint}"), paymentRequest))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _paymentApiClient.ProcessPayment(paymentRequest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Equal("Some error", result.Error);
            Assert.Empty(result.TransactionId);

            _loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                  Times.Once);
        }
    }
}
