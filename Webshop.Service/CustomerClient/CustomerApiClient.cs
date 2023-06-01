using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Service.CatalogClient;
using Webshop.Service.CatalogClient.Constants;
using Webshop.Service.CatalogClient.Exceptions;
using Webshop.Service.CatalogClient.Models.Responses.Internal;
using Webshop.Service.CatalogClient.Models.Responses;
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

        public CustomerApiClient(IHttpClientService client, ILogger<CustomerApiClient> logger, string baseUrl)
        {
            _client = client;
            _logger = logger;
            _baseUrl = baseUrl;
        }

        public async Task<GetCustomerResponse> GetCustomer(int id)
        {
            try
            {
                var response = await _client.GetAsync<CustomerResponse>($"{_baseUrl}/{CustomerEndpoints.Customers}/{id}");

                if (response.Result != null)
                {
                    _logger.LogInformation($"Successfully fetched customer with id: {id}.");
                    return new GetCustomerResponse
                    {
                        Status = "Success",
                        Customer = response.Result
                    };
                }

                _logger.LogInformation($"Failed to fetch customer with id: {id}.");
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
