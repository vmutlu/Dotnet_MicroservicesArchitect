using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.WEBUI.Models;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AuctionModel auctionModel)
        {
            return View(auctionModel);
        }

        public IActionResult Detail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Detail(AuctionModel auctionModel)
        {
            return View(auctionModel);
        }
    }
}
