using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.ProductAPI.DataAccess.Abstract;
using OnlineAuctionApp.ProductAPI.Entities;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OnlineAuctionApp.ProductAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _repository.GetAll().ConfigureAwait(false);

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(statusCode: (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            var product = await _repository.GetById(id).ConfigureAwait(false);

            if (product is null)
            {
                _logger.LogError($"Product with Id: {id}, hasn't been found in database");
                return NotFound(product);
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), statusCode: (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> Add([FromBody] Product product)
        {
            await _repository.Add(product).ConfigureAwait(false);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Update([FromBody] Product product)
        {
            var response = await _repository.Update(product).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Product), statusCode: (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(string id)
        {
            var response = await _repository.Delete(id).ConfigureAwait(false);

            return Ok(response);
        }
    }
}
