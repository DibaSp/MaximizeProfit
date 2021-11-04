using System;
using System.Collections.Generic;
using System.Linq;

namespace MaximizeProfitLib.Models
{
    public class Order
    {
        public Exchange Exchange { get; set; }
        public object Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Kind { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }

    public class Bid
    {
        public Order Order { get; set; }
    }

    public class Ask
    {
        public Order Order { get; set; }
    }

    public class Book
    {
        public DateTime AcqTime { get; set; }
        public List<Bid> Bids { get; set; }
        public List<Ask> Asks { get; set; }
        public List<Order> Orders { get; set; }

        public void SortOrders(string typeOfOrder)
        {
            if(typeOfOrder.ToLowerInvariant().Trim().Equals("sell"))
                Orders = Bids.OrderByDescending(b => b.Order.Price).Select(b => b.Order).ToList();
            else
                Orders = Asks.OrderBy(a => a.Order.Price).Select(a => a.Order).ToList();
        }
    }
}
