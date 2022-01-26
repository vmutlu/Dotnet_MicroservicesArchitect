using MongoDB.Driver;
using OnlineAuctionApp.ProductAPI.Entities;

namespace OnlineAuctionApp.ProductAPI.DataAccess.Abstract
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; set; }
    }
}
