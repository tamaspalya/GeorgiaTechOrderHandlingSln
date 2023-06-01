using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.PaymentClient.Models.Responses
{
    public class Payment
    {
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public int Cvc { get; set; }
    }
}
