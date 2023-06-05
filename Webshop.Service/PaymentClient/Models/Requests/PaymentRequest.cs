using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.PaymentClient.Models.Requests
{
    public class PaymentRequest
    {
        public string? CardNumber { get; set; }
        public string? ExpirationDate { get; set; }
        public int Cvc { get; set; }
        public int Amount { get; set; }
    }
}
