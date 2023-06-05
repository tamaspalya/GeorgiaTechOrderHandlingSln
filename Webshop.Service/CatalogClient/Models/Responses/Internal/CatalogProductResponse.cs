using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.CatalogClient.Models.Responses.Internal
{
    public class CatalogProductResponse
    {
        public ProductDto? Product { get; set; } = null;
        public string Status { get; set; } = string.Empty;
    }
}
