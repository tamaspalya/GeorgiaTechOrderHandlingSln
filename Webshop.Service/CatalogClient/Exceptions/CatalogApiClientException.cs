using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.CatalogClient.Exceptions
{
    public class CatalogApiClientException: Exception
    {
        public CatalogApiClientException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}
