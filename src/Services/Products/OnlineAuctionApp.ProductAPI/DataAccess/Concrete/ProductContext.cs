using MongoDB.Driver;
using OnlineAuctionApp.ProductAPI.DataAccess.Abstract;
using OnlineAuctionApp.ProductAPI.Entities;
using OnlineAuctionApp.ProductAPI.Settings.Abstract;

namespace OnlineAuctionApp.ProductAPI.DataAccess.Concrete
{
    public class ProductContext : IProductContext
    {
        public ProductContext(IProductDatabaseSettings productDatabaseSettings)
        {
            var client = new MongoClient(productDatabaseSettings.ConnectionStrings);
            var databaseClient = client.GetDatabase(productDatabaseSettings.DatabaseName);

            Products = databaseClient.GetCollection<Product>(productDatabaseSettings.CollectionName);
            SeedDatas.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; set; }
    }
}
