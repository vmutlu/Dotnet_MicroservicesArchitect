using OnlineAuctionApp.Domain.Entities.Abstract;
using System;

namespace OnlineAuctionApp.Domain.Entities.Concrete
{
    public class Order: Entity
    {
        public string AuctionId { get; set; }
        public string SellerUserName { get; set; }
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
