using AutoMapper;
using MediatR;
using OnlineAuctionApp.Application.Commands.OrderCreate;
using OnlineAuctionApp.Application.Responses;
using OnlineAuctionApp.Domain.DataAccess.Abstract;
using OnlineAuctionApp.Domain.Entities.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Application.Handlers
{
    public class OrderCreateHandler : IRequestHandler<OrderCreateCommand, OrderResponse>
    {
        private readonly IOrderedRepository _repository;
        private readonly IMapper _mapper;

        public OrderCreateHandler(IOrderedRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderResponse> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Order>(request);

            if (entity is null)
                throw new ApplicationException("Entity could not be mapped");

            var order = await _repository.AddAsync(entity).ConfigureAwait(false);

            var response = _mapper.Map<OrderResponse>(order);

            return response;
        }
    }
}
