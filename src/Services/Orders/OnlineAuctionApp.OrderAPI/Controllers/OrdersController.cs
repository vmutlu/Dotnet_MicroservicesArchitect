using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.Application.Commands.OrderCreate;
using OnlineAuctionApp.Application.Queries;
using OnlineAuctionApp.Application.Responses;
using OnlineAuctionApp.Domain.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineAuctionApp.OrderAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetOrdersByUserName/{userName}")]
        [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserName(string userName)
        {
            GetOrdersBySellerUserNameQuery query = new(userName);

            var orders = await _mediator.Send(query).ConfigureAwait(false);

            if (orders.Count() is decimal.Zero)
                return NotFound();

            return Ok(orders);
        }

        [HttpPost("GetOrdersByUserName")]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponse>> Create([FromBody] OrderCreateCommand orderCommand) =>
            Ok(await _mediator.Send(orderCommand).ConfigureAwait(false));
    }
}
