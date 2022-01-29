using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.AuctionAPI.DataAccess.Abstract;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OnlineBidApp.BidAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IBidRepository _repository;
        private readonly ILogger<BidsController> _logger;

        public BidsController(IBidRepository repository, ILogger<BidsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(typeof(IEnumerable<Bid>), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidByAuctionId(string id)
        {
            var bids = await _repository.GetBindsByAuctionId(id).ConfigureAwait(false);

            return Ok(bids);
        }

        [HttpGet("GetWinnerBid/{id:length(24)}")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Bid), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid(string id)
        {
            var bid = await _repository.GetWinnerBid(id).ConfigureAwait(false);

            if (bid is null)
            {
                _logger.LogError($"Bid with Id: {id}, hasn't been found in database");
                return NotFound(bid);
            }

            return Ok(bid);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.Created)]
        public async Task<ActionResult> SendBid([FromBody] Bid bid)
        {
            await _repository.SendBind(bid).ConfigureAwait(false);

            return Ok();
        }      
    }
}
