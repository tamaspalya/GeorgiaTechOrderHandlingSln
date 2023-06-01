using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.PaymentClient.Exceptions
{
    public class PaymentApiClientException: Exception
    {
        public PaymentApiClientException(string message, Exception innerException)
        : base(message, innerException)
        {
        }
    }
}
