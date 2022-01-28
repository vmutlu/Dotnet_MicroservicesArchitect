using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAuctionApp.AuctionAPI.DataAccess.Abstract
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<Auction>> GetAll();
        Task<Auction> GetById(string id);
        Task<IEnumerable<Auction>> GetAuctionByName(string name);
        Task Add(Auction Auction);
        Task<bool> Update(Auction Auction);
        Task<bool> Delete(string id);
    }
}
