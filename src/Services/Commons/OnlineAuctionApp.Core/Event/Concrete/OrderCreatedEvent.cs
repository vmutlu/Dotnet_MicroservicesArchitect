using OnlineAuctionApp.Core.Event.Abstract;
using System;

namespace OnlineAuctionApp.Core.Event.Concrete
{
    public class OrderCreatedEvent : IEvent
    {
        public string Id { get; set; }
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string SellerUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
