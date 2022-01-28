namespace OnlineAuctionApp.AuctionAPI.Settings.Abstract
{
    public interface ISourcingDatabaseSettings
    {
        public string ConnectionStrings { get; set; }
        public string DatabaseName { get; set; }
    }
}
