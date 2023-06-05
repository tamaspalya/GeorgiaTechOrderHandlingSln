using Webshop.Service.CustomerClient.Models;
using Webshop.Service.CustomerClient.Models.Responses.Internal;

namespace Webshop.Service.CustomerClient
{
    public interface ICustomerApiClient
    {
        public Task<GetCustomerResponse> GetCustomer(int id);
        public Task<bool> UpdateCustomer(CustomerDto customer);
    }
}
