using OnlineAuctionApp.ProductAPI.Settings.Abstract;

namespace OnlineAuctionApp.ProductAPI.Settings.Concrete
{
    public class ProductDatabaseSettings : IProductDatabaseSettings
    {
        public string ConnectionStrings { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
