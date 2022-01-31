using AutoMapper;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using OnlineAuctionApp.Core.Event.Concrete;

namespace OnlineAuctionApp.AuctionAPI.Mappings
{
    public class AuctionMap : Profile
    {
        public AuctionMap()
        {
            CreateMap<OrderCreatedEvent, Bid>().ReverseMap();
        }
    }
}
