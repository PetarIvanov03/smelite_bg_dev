using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using smelite_app.Models;
using smelite_app.Services;

namespace smelite_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSubscriptionService _subscriptionService;

        public HomeController(ILogger<HomeController> logger, IEmailSubscriptionService subscriptionService)
        {
            _logger = logger;
            _subscriptionService = subscriptionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Merch()
        {
            return View();
        }

        public IActionResult Mission()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult TermsAndConditions()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Notification"] = "Невалиден имейл";
                return RedirectToAction(nameof(Index));
            }
            await _subscriptionService.SubscribeAsync(email);
            TempData["Notification"] = "Благодарим за абонамента";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
