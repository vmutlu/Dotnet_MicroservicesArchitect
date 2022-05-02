using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.Core.DataAccess.Abstract;
using OnlineAuctionApp.Core.Response.Concrete;
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
        private readonly BidClient _bidClient;
        private readonly ILogger<AuctionController> _logger;
        private readonly IUserRepository _userRepository;
        public AuctionController(ILogger<AuctionController> logger, IUserRepository userRepository, AuctionClient auctionClient, ProductClient productClient, BidClient bidClient)
        {
            _logger = logger;
            _userRepository = userRepository;
            _auctionClient = auctionClient;
            _productClient = productClient;
            _bidClient = bidClient;
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
            auctionModel.Status = default(int);
            auctionModel.CreatedDate = DateTime.Now;

            var createAuction = await _auctionClient.CreateAuction(auctionModel).ConfigureAwait(false);
            if (createAuction.IsSuccess)
                return RedirectToAction("Index");

            return View(auctionModel);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var model = new AuctionBidModel();

            var auctionResponse = await _auctionClient.GetAuctionById(id).ConfigureAwait(false);
            var bidResponse = await _bidClient.GetAllBidsByAuctionId(id).ConfigureAwait(false);

            model.SellerUserName = HttpContext.User?.Identity?.Name;
            model.AuctionId = auctionResponse.Data.Id;
            model.ProductId = auctionResponse.Data.ProductId;
            model.Bids = bidResponse.Data;

            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            model.IsAdmin = Convert.ToBoolean(isAdmin);

            return View(model);
        }

        [HttpPost]
        public async Task<Result<string>> SendBid(BidModel model)
        {
            model.CreatedAt = DateTime.Now;

            var sendBidResponse = await _bidClient.SendBid(model).ConfigureAwait(false);

            return sendBidResponse;
        }
        
        [HttpPost]
        public async Task<Result<string>> CompleteBid(string id)
        {
            var completeBidResponse = await _auctionClient.CompleteBid(id).ConfigureAwait(false);

            return completeBidResponse;
        }
    }
}
