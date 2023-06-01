using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.CustomerClient.Exceptions
{
    public class CustomerApiClientException: Exception
    {
        public CustomerApiClientException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}
