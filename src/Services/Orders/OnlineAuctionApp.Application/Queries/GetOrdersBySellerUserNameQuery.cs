using MediatR;
using OnlineAuctionApp.Application.Responses;
using System.Collections.Generic;

namespace OnlineAuctionApp.Application.Queries
{
    public class GetOrdersBySellerUserNameQuery : IRequest<IEnumerable<OrderResponse>>
    {
        public string _userName { get; set; }
        public GetOrdersBySellerUserNameQuery(string userName)
        {
            _userName = userName;
        }
    }
}
