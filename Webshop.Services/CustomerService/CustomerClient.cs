using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.CustomerService.Models;
using Webshop.Services.CustomerService.Models.Responses;

namespace Webshop.Services.CustomerService
{
	public class CustomerClient
	{
        private readonly IHttpClientService _client;
        private const string API_URL = "http://localhost:8085/api/customers"; //TODO remove hard coded value

        public CustomerClient(IHttpClientService client)
		{
			_client = client;
		}

        public async Task<CustomerDto> GetCustomer(int id)
        {
            var response = await _client.GetAsync<GetCustomerResponse>($"{API_URL}/{id}");

            if (response != null && response.Result != null)
            {
                return response.Result;
            }

            throw new Exception("Failed to retrieve customer data");
        }
    }
}

