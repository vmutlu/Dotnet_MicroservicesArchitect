using MongoDB.Driver;
using OnlineAuctionApp.AuctionAPI.DataAccess.Abstract;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAuctionApp.AuctionAPI.DataAccess.Concrete
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly IAuctionContext _auctionContext;
        public AuctionRepository(IAuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }
        public async Task Add(Auction Auction)
        {
            await _auctionContext.Auctions.InsertOneAsync(Auction).ConfigureAwait(false);
        }

        public async Task<bool> Delete(string id)
        {
            var auction = Builders<Auction>.Filter.Eq(p => p.Id, id);
            var result = await _auctionContext.Auctions.DeleteOneAsync(auction).ConfigureAwait(false);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Auction>> GetAll()
        {
            return await _auctionContext.Auctions.Find(p => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<Auction> GetById(string id)
        {
            return await _auctionContext.Auctions.Find(p => p.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Auction>> GetAuctionByName(string name)
        {
            var auction = Builders<Auction>.Filter.ElemMatch(p => p.Name, name);

            return await _auctionContext.Auctions.Find(auction).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> Update(Auction Auction)
        {
            var result = await _auctionContext.Auctions.ReplaceOneAsync(filter: p => p.Id == Auction.Id, replacement: Auction).ConfigureAwait(false);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
