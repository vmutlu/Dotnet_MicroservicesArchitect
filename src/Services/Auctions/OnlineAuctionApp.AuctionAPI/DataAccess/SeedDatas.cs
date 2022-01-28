using MongoDB.Driver;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using System;
using System.Collections.Generic;

namespace OnlineAuctionApp.AuctionAPI.DataAccess
{
    public class SeedDatas
    {
        public static void SeedData(IMongoCollection<Auction> mongoCollection)
        {
            var exists = mongoCollection.Find(p => true).Any();
            if (!exists)
                mongoCollection.InsertManyAsync(GetConfigureAuctions());
        }

        private static IEnumerable<Auction> GetConfigureAuctions()
        {
            return new List<Auction>()
        {
            new Auction()
            {
                Name = "Auction One",
                CreatedDate = DateTime.Now,
                Description = "Example Auction One",
                FinishedDate = DateTime.Now.AddDays(3),
                ProductId = "61f193c8fa4d8942da2c6e72",
                Quantity = 5,
                StartedAt = DateTime.Now.AddDays(1),
                Status = Enums.EStatus.Active,
                Sellers = new List<string>()
                {
                    "veysel_mutlu42@hotmail.com",
                    "mutluveysel02@gmail.com",
                },
            },
            new Auction()
            {
                Name = "Auction Two",
                CreatedDate = DateTime.Now,
                Description = "Example Auction Two",
                FinishedDate = DateTime.Now.AddDays(3),
                ProductId = "61f193c8fa4d8942da2c6e73",
                Quantity = 5,
                StartedAt = DateTime.Now.AddDays(1),
                Status = Enums.EStatus.Active,
                Sellers = new List<string>()
                {
                    "veysel_mutlu42@hotmail.com",
                    "mutluveysel02@gmail.com",
                },
            },
            new Auction()
            {
                Name = "Auction Three",
                CreatedDate = DateTime.Now,
                Description = "Example Auction Three",
                FinishedDate = DateTime.Now.AddDays(3),
                ProductId = "61f193c8fa4d8942da2c6e74",
                Quantity = 5,
                StartedAt = DateTime.Now.AddDays(1),
                Status = Enums.EStatus.Active,
                Sellers = new List<string>()
                {
                    "veysel_mutlu42@hotmail.com",
                    "mutluveysel02@gmail.com",
                },
            }
        };
        }
    }
}
