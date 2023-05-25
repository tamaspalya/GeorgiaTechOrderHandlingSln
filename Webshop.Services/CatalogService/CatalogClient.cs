using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Services.CatalogService.Models;
using Webshop.Services.CatalogService.Models.Responses;

namespace Webshop.Services.CatalogService.Models
{
	public class CatalogClient
	{
        private readonly IHttpClientService _client;
        private const string API_URL = "http://localhost:8084/api/products"; //TODO remove hard coded value

        public CatalogClient(IHttpClientService client)
		{
			_client = client;
		}

        public async Task<ProductDto> GetProduct(int id)
        {
            var response = await _client.GetAsync<GetProductResponse>($"{API_URL}/{id}");

            if (response != null && response.Result != null)
            {
                return response.Result;
            }

            throw new Exception("Failed to retrieve product data");
        }

        public async Task<bool> UpdateProduct(ProductDto product)
        {
            var response = await _client.UpdateAsync<UpdateProductResponse>($"{API_URL}/{product.Id}", product);

            if (response != null && response.ErrorMessage == null)
            {
                return true;
            }

            throw new Exception("Failed to update product data");
        }

    }
}

