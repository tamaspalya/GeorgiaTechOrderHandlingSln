using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Domain.AggregateRoots
{
    public class Discount: AggregateRoot
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public int Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Discount(string type, string code, int value, DateTime startDate, DateTime endDate)
        {
            Type = type;
            Code = code;
            Value = value;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Discount()
        {
        }
    }
}
