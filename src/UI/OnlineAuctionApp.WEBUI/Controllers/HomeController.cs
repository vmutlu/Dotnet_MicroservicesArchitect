using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(loginModel.Email).ConfigureAwait(false);

                if (userExists is not null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(userExists, loginModel.Password, false, false).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetString("IsAdmin", userExists.IsAdmin.ToString());
                        return RedirectToAction("Index");
                    }

                    else
                        ModelState.AddModelError("", "Email address or password is not valid. 👌");
                }
            }
            return View(loginModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);

            return RedirectToAction("Login");
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
