using AutoMapper;
using MediatR;
using OnlineAuctionApp.Application.Queries;
using OnlineAuctionApp.Application.Responses;
using OnlineAuctionApp.Domain.DataAccess.Abstract;
using OnlineAuctionApp.Domain.Entities.Concrete;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Application.Handlers
{
    public class GetOrdersByUserNameHandler : IRequestHandler<GetOrdersBySellerUserNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderedRepository _repository;
        private readonly IMapper _mapper;

        public GetOrdersByUserNameHandler(IOrderedRepository repository, IMapper mapper = null)
        {
            _repository = repository;
            _mapper = mapper;
        }

        async Task<IEnumerable<OrderResponse>> IRequestHandler<GetOrdersBySellerUserNameQuery, IEnumerable<OrderResponse>>.Handle(GetOrdersBySellerUserNameQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetOrdersBySellerUserName(request._userName).ConfigureAwait(false);

            var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);

            return response;
        }
    }
}
