using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OnlineAuctionApp.WEBUI.Controllers
{
    public class AuctionController : Controller
    {
        private readonly ILogger<AuctionController> _logger;
        public AuctionController(ILogger<AuctionController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
