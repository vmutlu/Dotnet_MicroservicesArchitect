using System.Collections.Generic;

namespace OnlineAuctionApp.WEBUI.Models
{
    public class AuctionBidModel
    {
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string SellerUserName { get; set; }
        public bool IsAdmin { get; set; }
        public List<BidModel> Bids { get; set; }
    }
}
