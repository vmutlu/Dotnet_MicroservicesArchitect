using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.WEBUI.Models;
using System.Diagnostics;

namespace OnlineAuctionApp.WEBUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterModel registerModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        AppUser appUser = new();
        //        appUser.FirstName = registerModel.FirstName;
        //        appUser.LastName = registerModel.LastName;
        //        appUser.Email = registerModel.Email;
        //        appUser.PhoneNumber = registerModel.PhoneNumber;
        //        appUser.UserName = registerModel.UserName;

        //        switch (registerModel.UserSelectTypeId)
        //        {
        //            case 1:
        //                appUser.IsSeller = true;
        //                appUser.IsBuyer = false;
        //                break;
        //            case 2:
        //                appUser.IsSeller = false;
        //                appUser.IsBuyer = true;
        //                break;
        //        }

        //        var response = await _userManager.CreateAsync(appUser, registerModel.Password);
        //        if (response.Succeeded)
        //            return RedirectToAction("Login");

        //        else
        //            response.Errors.ToList().ForEach(i => { ModelState.AddModelError("", i.Description); });
        //    }

        //    return View(registerModel);
        //}
    }
}
