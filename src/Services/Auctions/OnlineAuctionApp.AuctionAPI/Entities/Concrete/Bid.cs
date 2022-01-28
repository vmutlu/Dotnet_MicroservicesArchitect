using MongoDB.Bson.Serialization.Attributes;
using System;

namespace OnlineAuctionApp.AuctionAPI.Entities.Concrete
{
    public class Bid
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string AuctionId { get; set; }
        public string ProductId { get; set; }
        public string SellerUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
    }
}
