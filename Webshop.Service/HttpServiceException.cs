using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service
{
    public class HttpServiceException: Exception
    {
        public HttpServiceException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}
