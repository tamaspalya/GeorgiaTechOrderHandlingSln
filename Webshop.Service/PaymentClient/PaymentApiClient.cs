using Microsoft.Extensions.Logging;
using Webshop.Service.PaymentClient.Models.Requests;
using Webshop.Service.PaymentClient.Models.Responses;
using Webshop.Service.PaymentClient.Constants;
using Webshop.Service.PaymentClient.Models.Responses.Internal;
using Webshop.Service.PaymentClient.Exceptions;

namespace Webshop.Service.PaymentClient
{
    public class PaymentApiClient : IPaymentApiClient
    {
        private readonly IHttpClientService _client;
        private readonly ILogger<PaymentApiClient> _logger;
        private readonly string _baseUrl;

        public PaymentApiClient(IHttpClientService client, ILogger<PaymentApiClient> logger, string baseUrl)
        {
            _client = client;
            _logger = logger;
            _baseUrl = baseUrl;
        }

        public async Task<ProcessPaymentResponse> ProcessPayment(PaymentRequest paymentRequest)
        {
            try
            {
                //Execute update
                var response = await _client.PostAsync<PaymentRequest, TransactionResult>($"{_baseUrl}/{PaymentEndpoints.ProcessPayment}", paymentRequest);

                if (response == null)
                {
                    string errorMessage = "Response from API was null";
                    _logger.LogError(errorMessage);
                    throw new ArgumentNullException(errorMessage);
                }

                if (response.IsSuccess)
                {
                    _logger.LogInformation("Payment successfully processed.");
                    return new ProcessPaymentResponse
                    {
                        IsSuccess = true,
                        IsFailure = false,
                        Error = string.Empty,
                        TransactionId = response.Value.TransactionId,
                    };
                }

                _logger.LogError($"Failed to process payment. Error was: {response.Error}");
                return new ProcessPaymentResponse 
                { 
                    IsSuccess = false,
                    IsFailure = true,
                    Error = response.Error,
                    TransactionId = string.Empty
                };
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occured while trying to process payment. Error message: {ex.Message}";
                _logger.LogError(errorMessage);
                throw new PaymentApiClientException(errorMessage, ex);
            }
        }
    }
}
