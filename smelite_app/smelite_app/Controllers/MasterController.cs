using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace smelite_app.Controllers
{
    [Authorize(Roles = "Master")]
    public class MasterController : Controller
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
