using AutoMapper;
using OnlineAuctionApp.Application.Commands.OrderCreate;
using OnlineAuctionApp.Application.Responses;
using OnlineAuctionApp.Domain.Entities.Concrete;

namespace OnlineAuctionApp.Application.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderCreateCommand>().ReverseMap();

            CreateMap<Order, OrderResponse>().ReverseMap();
        }
    }
}
