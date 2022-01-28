using MongoDB.Driver;
using OnlineAuctionApp.AuctionAPI.DataAccess.Abstract;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using OnlineAuctionApp.AuctionAPI.Settings.Abstract;

namespace OnlineAuctionApp.AuctionAPI.DataAccess.Concrete
{
    public class AuctionContext : IAuctionContext
    {
        public AuctionContext(ISourcingDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionStrings);
            var databaseClient = client.GetDatabase(databaseSettings.DatabaseName);

            Auctions = databaseClient.GetCollection<Auction>(nameof(Auction));
            Bids = databaseClient.GetCollection<Bid>(nameof(Bid));
        }
        public IMongoCollection<Auction> Auctions { get; set; }
        public IMongoCollection<Bid> Bids { get; set; }
    }
}
