using MongoDB.Driver;
using OnlineAuctionApp.ProductAPI.Entities;
using System.Collections.Generic;

namespace OnlineAuctionApp.ProductAPI.DataAccess
{
    public class SeedDatas
    {
        public static void SeedData(IMongoCollection<Product> mongoCollection)
        {
            var exists = mongoCollection.Find(p => true).Any();
            if (!exists)
                mongoCollection.InsertManyAsync(GetConfigureProducts());
        }

        private static IEnumerable<Product> GetConfigureProducts()
        {
            return new List<Product>()
        {
            new Product()
            {
                Name = "Example Product One",
                Summary = "Example Product",
                Description =   "Example Products for my application",
                ImageFile ="product-1.png",
                Price =100,
                Category="Category 1"
            },
            new Product()
            {
                Name = "Example Product Two",
                Summary = "Example Product",
                Description =   "Example Products for my application",
                ImageFile ="product-2.png",
                Price =200,
                Category="Category 2"
            },
            new Product()
            {
                Name = "Example Product Three",
                Summary = "Example Product",
                Description =   "Example Products for my application",
                ImageFile ="product-3.png",
                Price =300,
                Category="Category 3"
            }
        };
        }
    }
}
