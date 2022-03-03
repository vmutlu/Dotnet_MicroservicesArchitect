using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.Core.DataAccess.Abstract;
using OnlineAuctionApp.WEBUI.Clients;
using OnlineAuctionApp.WEBUI.Models;
using System;
using System.Threading.Tasks;

namespace OnlineAuctionApp.WEBUI.Controllers
{
    public class AuctionController : Controller
    {
        private readonly AuctionClient _auctionClient;
        private readonly ProductClient _productClient;
        private readonly ILogger<AuctionController> _logger;
        private readonly IUserRepository _userRepository;
        public AuctionController(ILogger<AuctionController> logger, IUserRepository userRepository, AuctionClient auctionClient, ProductClient productClient)
        {
            _logger = logger;
            _userRepository = userRepository;
            _auctionClient = auctionClient;
            _productClient = productClient;
        }

        public async Task<IActionResult> Index()
        {
            var productList = await _productClient.GetProducts().ConfigureAwait(false);
            if (productList.IsSuccess)
                ViewBag.ProductList = productList.Data;

            var userList = await _userRepository.GetAllAsync().ConfigureAwait(false);
            ViewBag.UserList = userList;

            var auctionList = await _auctionClient.GetAuctions().ConfigureAwait(false);
            if (auctionList.IsSuccess)
                return View(auctionList.Data);

            else
                return View();
        }

        public async Task<IActionResult> Create()
        {
            var productList = await _productClient.GetProducts().ConfigureAwait(false);

            if (productList.IsSuccess)
                ViewBag.ProductList = productList.Data;

            var userList = await _userRepository.GetAllAsync().ConfigureAwait(false);
            ViewBag.UserList = userList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionModel auctionModel)
        {
            auctionModel.Status = (int)decimal.One;
            auctionModel.CreatedDate = DateTime.Now;

            var createAuction = await _auctionClient.CreateAuction(auctionModel).ConfigureAwait(false);
            if (createAuction.IsSuccess)
                return RedirectToAction("Index");

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
