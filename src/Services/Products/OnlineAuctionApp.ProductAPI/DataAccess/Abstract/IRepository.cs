using OnlineAuctionApp.ProductAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAuctionApp.ProductAPI.DataAccess.Abstract
{
    public interface IRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(string id);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
        Task Add(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(string id);
    }
}
