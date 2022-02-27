using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.Core.DataAccess.Abstract;
using OnlineAuctionApp.WEBUI.Models;
using System.Threading.Tasks;

namespace OnlineAuctionApp.WEBUI.Controllers
{
    public class AuctionController : Controller
    {
        private readonly ILogger<AuctionController> _logger;
        private readonly IUserRepository _userRepository;
        public AuctionController(ILogger<AuctionController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var productList = await _productClient.GetProducts();

            if (productList.IsSuccess)
                ViewBag.ProductList = productList.Data;

            var userList = await _userRepository.GetAllAsync().ConfigureAwait(false);
            ViewBag.UserList = userList;

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
