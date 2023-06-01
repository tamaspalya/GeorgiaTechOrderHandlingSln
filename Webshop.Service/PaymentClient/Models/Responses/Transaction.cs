using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.PaymentClient.Models.Responses
{
    public class Transaction
    {
        public string Created { get; set; }
        public int Amount { get; set; }
        public Payment Payment { get; set; }
        public string TransactionId { get; set; }
    }
}
