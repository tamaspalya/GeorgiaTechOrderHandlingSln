using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Service.CatalogClient.Models;

namespace Webshop.Service.CustomerClient.Models.Responses.Internal
{
    public class GetCustomerResponse
    {
        public CustomerDto? Customer { get; set; } = null;
        public string Status { get; set; } = string.Empty;
    }
}
