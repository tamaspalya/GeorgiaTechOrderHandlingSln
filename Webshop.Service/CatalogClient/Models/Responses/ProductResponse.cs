using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Service.Common;

namespace Webshop.Service.CatalogClient.Models.Responses
{
    public class ProductResponse: BaseResponse
    {
        public ProductDto Result { get; set; }
    }
}
