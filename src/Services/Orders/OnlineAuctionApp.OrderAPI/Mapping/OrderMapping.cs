using AutoMapper;
using OnlineAuctionApp.Application.Commands.OrderCreate;
using OnlineAuctionApp.Core.Event.Concrete;

namespace OnlineAuctionApp.OrderAPI.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping() => CreateMap<OrderCreatedEvent, OrderCreateCommand>().ReverseMap();
    }
}
