using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace smelite_app.Controllers
{
    [Authorize(Roles = "Apprentice")]
    public class ApprenticeController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult CreateCraft()
        {
            return View();
        }

        public IActionResult Sessions()
        {
            return View();
        }
    }
}
