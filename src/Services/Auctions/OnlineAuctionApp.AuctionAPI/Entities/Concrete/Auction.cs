using MongoDB.Bson.Serialization.Attributes;
using OnlineAuctionApp.AuctionAPI.Enums;
using System;
using System.Collections.Generic;

namespace OnlineAuctionApp.AuctionAPI.Entities.Concrete
{
    public class Auction
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public EStatus Status { get; set; }
        public List<string> Sellers { get; set; }
    }
}
