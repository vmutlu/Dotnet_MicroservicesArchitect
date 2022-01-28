using MongoDB.Driver;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;

namespace OnlineAuctionApp.AuctionAPI.DataAccess.Abstract
{
    public interface IAuctionContext
    {
        IMongoCollection<Auction> Auctions { get; set; }
        IMongoCollection<Bid> Bids { get; set; }
    }
}
