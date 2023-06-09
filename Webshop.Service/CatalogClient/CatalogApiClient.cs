﻿using Microsoft.Extensions.Logging;
using Webshop.Service.CatalogClient.Constants;
using Webshop.Service.CatalogClient.Exceptions;
using Webshop.Service.CatalogClient.Models;
using Webshop.Service.CatalogClient.Models.Requests;
using Webshop.Service.CatalogClient.Models.Responses;
using Webshop.Service.CatalogClient.Models.Responses.Internal;

namespace Webshop.Service.CatalogClient
{
    public class CatalogApiClient: ICatalogApiClient
    {
        private readonly IHttpClientService _client;
        private readonly ILogger<CatalogApiClient> _logger;
        private readonly string _baseUrl;

        public CatalogApiClient(IHttpClientService client, ILogger<CatalogApiClient> logger, CatalogApiClientOptions options)
        {
            _client = client;
            _logger = logger;
            _baseUrl = options.BaseUrl;
        }

        public async Task<CatalogProductResponse> CreateProduct(CreateProductRequest createProductRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<CatalogProductResponse> GetProduct(int id)
        {
            try
            {
                var response = await _client.GetAsync<ProductResponse>($"{_baseUrl}/{CatalogEndpoints.Products}/{id}");

                if (response.Result != null)
                {
                    _logger.LogInformation($"Successfully fetched product with id: {id} from Catalog.");
                    return new CatalogProductResponse
                    {
                        Status = "Success",
                        Product = response.Result
                    };
                }

                _logger.LogError($"Failed to fetch product with id: {id} from Catalog.");
                return new CatalogProductResponse
                {
                    Status = "Fail"
                };
            }
            catch (Exception ex)
            {
                throw new CatalogApiClientException($"Error while fetching product with id: {id} from Catalog.", ex);
            }
        }

        public async Task<bool> UpdateProduct(ProductDto product)
        {
            try
            {
                //Execute update
                await _client.UpdateAsync<ProductDto, ProductResponse>($"{_baseUrl}/{CatalogEndpoints.Products}/{product.Id}", product);

                //Fetch product to see changes
                var response = await GetProduct(product.Id);

                //Check if update was successful
                if (response.Product != null && response.Product.Equals(product))
                {
                    _logger.LogInformation($"Successfully updated product with id: {product.Id}");
                    return true;
                }
                
                _logger.LogWarning($"Product with {product.Id} was not found or update failed.");
                return false;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occured while trying to update product with id: {product.Id} from Catalog.";
                _logger.LogError(errorMessage);
                throw new CatalogApiClientException(errorMessage, ex);
            }
        }
    }
}
