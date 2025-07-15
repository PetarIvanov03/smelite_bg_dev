using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smelite_app.Models;
using smelite_app.Data;
using smelite_app.ViewModels.Account;
using smelite_app.Services;

namespace smelite_app.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountService.RegisterAsync(model);
            if (result.Succeeded)
            {
                return View("CheckEmail");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountService.LoginAsync(model);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");

            var result = await _accountService.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
                return View("ConfirmEmail");
            else
                return View("Error");
        }
    }
}
