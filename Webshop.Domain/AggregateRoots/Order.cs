﻿using Webshop.Domain.Common;

namespace Webshop.Domain.AggregateRoots
{
    public class Order: AggregateRoot
    {
        public Order()
        {
            //For ORM
        }

        public Order(int customerId, DateTime orderDate, double totalPrice, string orderStatus, int sellerId, int discountId, List<OrderLineItem> orderLineItems)
        {
            CustomerId = customerId;
            OrderDate = orderDate;
            TotalPrice = totalPrice;
            OrderStatus = orderStatus;
            SellerId = sellerId;
            DiscountId = discountId;
            OrderLineItems = orderLineItems;
        }

        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public int SellerId { get; set; }
        public int DiscountId { get; set; }
        public List<OrderLineItem> OrderLineItems { get; set; }
    }
}
