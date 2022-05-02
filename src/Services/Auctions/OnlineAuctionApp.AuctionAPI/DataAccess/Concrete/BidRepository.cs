using MongoDB.Driver;
using OnlineAuctionApp.AuctionAPI.DataAccess.Abstract;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuctionApp.AuctionAPI.DataAccess.Concrete
{
    public class BidRepository : IBidRepository
    {
        private readonly IAuctionContext _auctionContext;
        public BidRepository(IAuctionContext auctionContext)
        {
            _auctionContext = auctionContext;
        }

        /// <summary>
        /// User Last Bids
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Bid>> GetBindsByAuctionId(string id)
        {
            var bid = Builders<Bid>.Filter.Eq(p => p.AuctionId, id);

            var bids = await _auctionContext.Bids.Find(bid).ToListAsync().ConfigureAwait(false);

            bids = bids.OrderByDescending(a => a.CreatedDate).GroupBy(a => a.SellerUserName).Select(u => new Bid()
            {
                AuctionId = u.FirstOrDefault().AuctionId,
                Price = u.FirstOrDefault().Price,
                CreatedDate = u.FirstOrDefault().CreatedDate,
                SellerUserName = u.FirstOrDefault().SellerUserName,
                ProductId = u.FirstOrDefault().ProductId,
                Id = u.FirstOrDefault().Id
            }).ToList();

            return bids;
        }

        public async Task<List<Bid>> GetAllBidsByAuctionId(string id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(p => p.AuctionId, id);

            List<Bid> bids = await _auctionContext
                          .Bids
                          .Find(filter)
                          .ToListAsync();

            bids = bids.OrderByDescending(a => a.CreatedDate)
                                   .Select(a => new Bid
                                   {
                                       AuctionId = a.AuctionId,
                                       Price = a.Price,
                                       CreatedDate = a.CreatedDate,
                                       SellerUserName = a.SellerUserName,
                                       ProductId = a.ProductId,
                                       Id = a.Id
                                   }).ToList();

            return bids;
        }

        /// <summary>
        /// Winning Bid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Bid> GetWinnerBid(string id)
        {
            var bids = await GetBindsByAuctionId(id).ConfigureAwait(false);

            return bids.OrderByDescending(a => a.Price).FirstOrDefault();
        }

        public async Task SendBind(Bid bid)
        {
            await _auctionContext.Bids.InsertOneAsync(bid).ConfigureAwait(false);
        }
    }
}
