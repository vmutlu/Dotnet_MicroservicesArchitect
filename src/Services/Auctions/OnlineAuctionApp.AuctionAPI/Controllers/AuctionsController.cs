using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.AuctionAPI.DataAccess.Abstract;
using OnlineAuctionApp.AuctionAPI.Entities.Concrete;
using OnlineAuctionApp.AuctionAPI.Enums;
using OnlineAuctionApp.Core.Common;
using OnlineAuctionApp.Core.Event.Concrete;
using OnlineAuctionApp.Core.Producer;
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
        private readonly IBidRepository _bidRepository;
        private readonly RabbitMQProducer _rabbitMQProducer;
        private readonly IMapper _mapper;
        private readonly ILogger<AuctionsController> _logger;

        public AuctionsController(IAuctionRepository repository, ILogger<AuctionsController> logger, IBidRepository bidRepository, RabbitMQProducer rabbitMQProducer, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _bidRepository = bidRepository;
            _rabbitMQProducer = rabbitMQProducer;
            _mapper = mapper;
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

        [HttpPost("CompleteAuction/{id:length(24)}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> CompleteAuction(string id)
        {
            var auction = await _repository.GetById(id).ConfigureAwait(false);
            if (auction is null)
                return NotFound();

            if (auction.Status != EStatus.Active)
            {
                _logger.LogError("Auction tamamlanamadı.");
                return BadRequest();
            }

            // Kazanan teklifi çekiyorum
            var bid = await _bidRepository.GetWinnerBid(id).ConfigureAwait(false);
            if (bid is null)
                return NotFound();

            var events = _mapper.Map<OrderCreatedEvent>(bid);
            events.Quantity = auction.Quantity;

            auction.Status = EStatus.Closed;
            var updated = await _repository.Update(auction).ConfigureAwait(false);

            if (!updated)
            {
                _logger.LogError("Auction tamamlanamadı.");
                return BadRequest();
            }

            try
            {
                _rabbitMQProducer.Publish(EventBusConstants.ORDER_CREATE, events); // Kazanan teklifin mesajını bıraktım. İlgili Q ya ilgili event bırakıldı.
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, " Hata {EventId} from {AppName}" + events.Id, "Sourcing");
                throw;
            }

            return Accepted();
        }

        [HttpPost("TestEvent")]
        public ActionResult<OrderCreatedEvent> TestEvent()
        {
            OrderCreatedEvent testEvent = new();
            testEvent.AuctionId = "dummy1";
            testEvent.ProductId = "dummy_product_1";
            testEvent.Price = 10;
            testEvent.Quantity = 100;
            testEvent.SellerUserName = "test@test.com";

            try
            {
                _rabbitMQProducer.Publish(EventBusConstants.ORDER_CREATE, testEvent); // Kazanan teklifin mesajını bıraktım. İlgili Q ya ilgili event bırakıldı.
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, " Hata {EventId} from {AppName}" + testEvent.Id, "Sourcing");
                throw;
            }

            return Accepted();
        }
    }
}
