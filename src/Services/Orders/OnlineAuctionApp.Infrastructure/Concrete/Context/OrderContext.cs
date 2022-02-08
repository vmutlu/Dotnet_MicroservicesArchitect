using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Domain.Entities.Concrete;

namespace OnlineAuctionApp.Infrastructure.Concrete.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
    }
}
