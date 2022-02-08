using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Domain.DataAccess.Abstract;
using OnlineAuctionApp.Domain.Entities.Concrete;
using OnlineAuctionApp.Infrastructure.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Infrastructure.DataAccess.Concrete
{
    public class OrderedRepository : Repository<Order>, IOrderedRepository
    {
        public OrderedRepository(OrderContext orderContext) : base(orderContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string userName) => 
            await _orderContext.Orders
            .Where(go => go.SellerUserName == userName)
            .ToListAsync()
            .ConfigureAwait(false);
    }
}
