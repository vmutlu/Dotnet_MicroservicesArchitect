using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAuctionApp.AuctionAPI.DataAccess.Abstract
{
    public interface IBidRepository
    {
        Task<IEnumerable<Bid>> GetBindsByAuctionId(string id);
        Task<List<Bid>> GetAllBidsByAuctionId(string id);
        Task SendBind(Bid bid);
        Task<Bid> GetWinnerBid(string id);
    }
}
