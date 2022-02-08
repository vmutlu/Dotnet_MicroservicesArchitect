using OnlineAuctionApp.Domain.Entities.Concrete;
using OnlineAuctionApp.Infrastructure.Concrete.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Infrastructure.Concrete.Seeds
{
    public class OrderSeed
    {
        public static async Task SeedAsync(OrderContext orderContex)
        {
            if (!orderContex.Orders.Any())
            {
                orderContex.Orders.AddRange(GetSeedOrders());
                await orderContex.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        private static List<Order> GetSeedOrders() =>
            new List<Order>()
            {
                new Order()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "veysel_mutlu42@hotmail.com",
                    UnitPrice = 10,
                    TotalPrice = 100,
                    CreatedDate = DateTime.UtcNow
                },
                new Order()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "veysel_mutlu42@hotmail.com",
                    UnitPrice = 8,
                    TotalPrice = 100,
                    CreatedDate = DateTime.UtcNow.AddDays(1)
                },
                new Order()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "veysel_mutlu42@hotmail.com",
                    UnitPrice = 5,
                    TotalPrice = 100,
                    CreatedDate = DateTime.UtcNow.AddDays(2)
                }
            };
    }
}
