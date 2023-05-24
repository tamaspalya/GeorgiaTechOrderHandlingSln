using Webshop.Domain.Common;

namespace Webshop.Domain.AggregateRoots
{
    public class Order: AggregateRoot
    {
        public Order()
        {
            //For ORM
        }

        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public int SellerId { get; set; }
        public int DiscountId { get; set; }
    }
}
