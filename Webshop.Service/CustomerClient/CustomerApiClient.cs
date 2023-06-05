using Microsoft.Extensions.Logging;
using Webshop.Service.CustomerClient.Models;
using Webshop.Service.CustomerClient.Models.Responses.Internal;
using Webshop.Service.CustomerClient.Models.Responses;
using Webshop.Service.CustomerClient.Constants;
using Webshop.Service.CustomerClient.Exceptions;

namespace Webshop.Service.CustomerClient
{
    public class CustomerApiClient : ICustomerApiClient
    {
        private readonly IHttpClientService _client;
        private readonly ILogger<CustomerApiClient> _logger;
        private readonly string _baseUrl;

        public CustomerApiClient(IHttpClientService client, ILogger<CustomerApiClient> logger, CustomerApiClientOptions options)
        {
            _client = client;
            _logger = logger;
            _baseUrl = options.BaseUrl;
        }

        public async Task<GetCustomerResponse> GetCustomer(int id)
        {
            try
            {
                var response = await _client.GetAsync<CustomerResponse>($"{_baseUrl}/{CustomerEndpoints.Customers}/{id}");

                if (response == null)
                {
                    string errorMessage = $"Response from the API was null.";
                    _logger.LogError(errorMessage);
                    throw new ArgumentNullException(errorMessage);
                }
                
                if (response.Result != null)
                {
                    _logger.LogInformation($"Successfully fetched customer with id: {id}.");
                    return new GetCustomerResponse
                    {
                        Status = "Success",
                        Customer = response.Result
                    };
                }

                _logger.LogError($"Failed to fetch customer with id: {id}.");
                return new GetCustomerResponse
                {
                    Status = "Fail"
                };
            }
            catch (Exception ex)
            {
                throw new CustomerApiClientException($"Error occured while fetching customer with id: {id}.", ex);
            }
        }

        public Task<bool> UpdateCustomer(CustomerDto customer)
        {
            throw new NotImplementedException();
        }
    }
}
