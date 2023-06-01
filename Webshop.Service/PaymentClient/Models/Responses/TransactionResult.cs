using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Service.PaymentClient.Models.Responses
{
    public class TransactionResult
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public bool IsFailure { get; set; }
        public Transaction Value { get; set; }
    }
}
