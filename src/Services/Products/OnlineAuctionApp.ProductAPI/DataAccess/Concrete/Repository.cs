using MongoDB.Driver;
using OnlineAuctionApp.ProductAPI.DataAccess.Abstract;
using OnlineAuctionApp.ProductAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAuctionApp.ProductAPI.DataAccess.Concrete
{
    public class Repository : IRepository
    {
        private readonly IProductContext _productContext;
        public Repository(IProductContext productContext)
        {
            _productContext = productContext;
        }
        public async Task Add(Product product)
        {
            await _productContext.Products.InsertOneAsync(product).ConfigureAwait(false);
        }

        public async Task<bool> Delete(string id)
        {
            var product = Builders<Product>.Filter.Eq(p => p.Id, id);
            var result = await _productContext.Products.DeleteOneAsync(product).ConfigureAwait(false);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productContext.Products.Find(p => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Product> GetById(string id)
        {
            return await _productContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var product = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _productContext.Products.Find(product).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var product = Builders<Product>.Filter.ElemMatch(p => p.Name, name);

            return await _productContext.Products.Find(product).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> Update(Product product)
        {
            var result = await _productContext.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product).ConfigureAwait(false);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
