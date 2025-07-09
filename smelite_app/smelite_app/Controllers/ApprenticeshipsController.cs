using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;

namespace smelite_app.Controllers
{
    public class ApprenticeshipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApprenticeshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int orderId)
        {
            var order = await _context.CraftOrders
                .Include(o => o.CraftOffering)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.Masters = await _context.MasterProfiles.ToListAsync();
            var apprenticeship = new Apprenticeship
            {
                ApprenticeProfileId = order.ApprenticeProfileId,
                CraftId = order.CraftOffering.CraftId,
                CraftOrderId = order.Id
            };
            return View(apprenticeship);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Apprenticeship apprenticeship)
        {
            if (ModelState.IsValid)
            {
                _context.Apprenticeships.Add(apprenticeship);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = apprenticeship.Id });
            }
            ViewBag.Masters = await _context.MasterProfiles.ToListAsync();
            return View(apprenticeship);
        }

        public async Task<IActionResult> Details(int id)
        {
            var apprenticeship = await _context.Apprenticeships
                .Include(a => a.Craft)
                .Include(a => a.MasterProfile)
                .Include(a => a.ApprenticeProfile)
                .Include(a => a.CraftOrder)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (apprenticeship == null)
            {
                return NotFound();
            }
            return View(apprenticeship);
        }
    }
}
