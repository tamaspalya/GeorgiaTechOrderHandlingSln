using Microsoft.Extensions.Logging;
using Moq;
using Webshop.Service.CustomerClient;
using Webshop.Service.CustomerClient.Models;
using Webshop.Service.PaymentClient;
using Webshop.Service.PaymentClient.Models.Requests;

namespace Webshop.Service.Test.Integration
{
    public class PaymentApiClientTests
    {
        private readonly IPaymentApiClient _client;
        private readonly HttpClientService _httpClientService;
        private readonly string _baseUrl = "http://localhost:8083/api";

        private PaymentRequest _testPaymentRequest;

        public PaymentApiClientTests()
        {
            var logger = new Mock<ILogger<PaymentApiClient>>();
            _httpClientService = new HttpClientService(new HttpClient());
            _client = new PaymentApiClient(_httpClientService, logger.Object, _baseUrl);
        }

        private void Setup()
        {
            _testPaymentRequest = new PaymentRequest
            {
                CardNumber = "1234567890123456",
                ExpirationDate = "12/25",
                Cvc = 123,
                Amount = GenerateRandomAmount()
            };
        }

        private void Teardown()
        {
            _testPaymentRequest = null;
        }

        [Fact]
        public async Task ProcessPaymentSuccessfully()
        {
            Setup();
            //Arrange

            //Act
            var response = await _client.ProcessPayment(_testPaymentRequest);
            //Assert

            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.False(response.IsFailure);
            Assert.Equal(response.Error, string.Empty);

            Teardown();
        }

        [Fact]
        public async Task ProcessPaymentFails_IfTwoPaymentsWithSameCardNumberAndAmountAreSubmitted()
        {
            Setup();
            //Arrange
            string duplicateTransactionErrorMessage = "A transaction with these parameters already exists. For security reasons two identical transactions cannot be processed (Amount and Cardnumber)";
            _testPaymentRequest.Amount = 10;

            //Act
            //Process two payments so that one must fail
            await _client.ProcessPayment(_testPaymentRequest);
            var response = await _client.ProcessPayment(_testPaymentRequest);
            //Assert

            Assert.NotNull(response);
            Assert.False(response.IsSuccess);
            Assert.True(response.IsFailure);
            Assert.Equal(response.Error, duplicateTransactionErrorMessage);

            Teardown();
        }

        #region Helper methods
        private int GenerateRandomAmount()
        {
            Random random = new Random();
            return random.Next(1, 10000);
        }
        #endregion
    }
}
