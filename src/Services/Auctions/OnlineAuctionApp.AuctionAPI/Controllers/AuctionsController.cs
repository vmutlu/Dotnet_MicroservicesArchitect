using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.AuctionAPI.DataAccess.Abstract;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OnlineAuctionApp.AuctionAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionRepository _repository;
        private readonly ILogger<AuctionsController> _logger;

        public AuctionsController(IAuctionRepository repository, ILogger<AuctionsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAll()
        {
            var auctions = await _repository.GetAll().ConfigureAwait(false);

            return Ok(auctions);
        }

        [HttpGet("{id:length(24)}", Name = "GetAuction")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Auction), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> GetById(string id)
        {
            var auction = await _repository.GetById(id).ConfigureAwait(false);

            if (auction is null)
            {
                _logger.LogError($"Auction with Id: {id}, hasn't been found in database");
                return NotFound(auction);
            }

            return Ok(auction);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Auction), statusCode: (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Auction>> Add([FromBody] Auction Auction)
        {
            await _repository.Add(Auction).ConfigureAwait(false);

            return CreatedAtRoute("GetAuction", new { id = Auction.Id }, Auction);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Auction), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> Update([FromBody] Auction Auction)
        {
            var response = await _repository.Update(Auction).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(string id)
        {
            var response = await _repository.Delete(id).ConfigureAwait(false);

            return Ok(response);
        }
    }
}
