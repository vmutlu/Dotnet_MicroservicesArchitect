using Microsoft.EntityFrameworkCore;

namespace OnlineAuctionApp.Infrastructure.Concrete.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }
    }
}
