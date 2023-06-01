using Webshop.Service.CatalogClient.Models.Responses.Internal;
using Webshop.Service.CatalogClient.Models;

namespace Webshop.Service.CatalogClient
{
    public interface ICatalogApiClient
    {
        public Task<CatalogProductResponse> GetProduct(int id);
        public Task<bool> UpdateProduct(ProductDto product);
    }
}
