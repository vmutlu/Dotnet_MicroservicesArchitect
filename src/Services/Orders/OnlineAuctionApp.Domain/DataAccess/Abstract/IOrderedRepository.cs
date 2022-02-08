using OnlineAuctionApp.Domain.Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Domain.DataAccess.Abstract
{
    public interface IOrderedRepository: IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersBySellerUserName(string userName);
    }
}
