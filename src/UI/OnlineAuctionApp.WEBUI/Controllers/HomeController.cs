using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.Core.Entities;
using OnlineAuctionApp.WEBUI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuctionApp.WEBUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var response = await AddUserAsync(registerModel).ConfigureAwait(false);

                if (response.Succeeded)
                    return RedirectToAction("Login");

                else
                    response.Errors.ToList().ForEach(i => { ModelState.AddModelError("", i.Description); });
            }

            return View(registerModel);
        }

        private async Task<IdentityResult> AddUserAsync(RegisterModel registerModel)
        {
            ApplicationUser appUser = new();
            appUser.FirstName = registerModel.FirstName;
            appUser.LastName = registerModel.LastName;
            appUser.Email = registerModel.Email;
            appUser.PhoneNumber = registerModel.PhoneNumber;
            appUser.UserName = registerModel.UserName;

            switch (registerModel.UserSelectType)
            {
                case Enums.UserType.Buyer:
                    appUser.IsSeller = true;
                    appUser.IsBuyer = false;
                    break;
                case Enums.UserType.Seller:
                    appUser.IsSeller = false;
                    appUser.IsBuyer = true;
                    break;
            }

            return await _userManager.CreateAsync(appUser, registerModel.Password);            
        }
    }
}
