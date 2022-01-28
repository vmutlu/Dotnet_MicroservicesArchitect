using OnlineAuctionApp.AuctionAPI.Settings.Abstract;

namespace OnlineAuctionApp.AuctionAPI.Settings.Concrete
{
    public class SourcingDatabaseSettings : ISourcingDatabaseSettings
    {
        public string ConnectionStrings { get; set; }
        public string DatabaseName { get; set; }
    }
}
