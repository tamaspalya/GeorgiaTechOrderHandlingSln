using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Service.CatalogClient.Models;
using Webshop.Service.CatalogClient.Models.Responses;

namespace Webshop.Service.CatalogClient
{
    public class CatalogApiClient
    {
        private readonly IHttpClientService _client;
        private readonly ILogger<CatalogApiClient> _logger;
        private readonly string _baseUrl;
        private const string API_URL = "http://localhost:8084/api/products"; //TODO remove hard coded value

        public CatalogApiClient(IHttpClientService client, ILogger<CatalogApiClient> logger, string baseUrl)
        {
            _client = client;
            _logger = logger;
            _baseUrl = baseUrl;
        }

        public async Task<ProductResponse> GetProduct(int id)
        {   
            var response = await _client.GetAsync<ProductResponse>($"{_baseUrl}/{CatalogEndpoints.Products}/{id}");

            if (response.Result == null)
            {
                _logger.LogError($"Failed to fetch product with id: {id} from Catalog.");
            } 
            else
            {
                _logger.LogInformation($"Successfully fetched product with id: {id} from Catalog.");
            }

            return response;
        }

        public async Task<bool> UpdateProduct(ProductDto product)
        {
            //Execute update
            await _client.UpdateAsync<ProductDto, ProductResponse>($"{_baseUrl}/{CatalogEndpoints.Products}/{product.Id}", product);

            try
            {
                //Fetch product to see changes
                var updateResponse = await GetProduct(product.Id);

                //Compare old and updated product
                if (updateResponse.Result.Equals(product))
                {
                    _logger.LogInformation($"Successfully updated product with id: {product.Id}");
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error occured while updating product: {e}");
            }

            _logger.LogError($"Failed to update product with id: {product.Id}");
            return false;
        }
    }
}
