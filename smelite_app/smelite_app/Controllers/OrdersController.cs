using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;

namespace smelite_app.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CraftOfferings = await _context.CraftOfferings
                .Include(o => o.Craft)
                .Include(o => o.CraftLocation)
                .Include(o => o.CraftPackage)
                .ToListAsync();
            ViewBag.Apprentices = await _context.ApprenticeProfiles.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CraftOrder order)
        {
            if (ModelState.IsValid)
            {
                _context.CraftOrders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = order.Id });
            }
            ViewBag.CraftOfferings = await _context.CraftOfferings
                .Include(o => o.Craft)
                .Include(o => o.CraftLocation)
                .Include(o => o.CraftPackage)
                .ToListAsync();
            ViewBag.Apprentices = await _context.ApprenticeProfiles.ToListAsync();
            return View(order);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.CraftOrders
                .Include(o => o.CraftOffering)
                    .ThenInclude(co => co.Craft)
                .Include(o => o.CraftOffering.CraftLocation)
                .Include(o => o.CraftOffering.CraftPackage)
                .Include(o => o.ApprenticeProfile)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
