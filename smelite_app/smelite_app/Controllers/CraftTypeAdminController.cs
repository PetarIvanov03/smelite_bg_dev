using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smelite_app.Models;
using smelite_app.Services;
using System.Globalization;

namespace smelite_app.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CraftTypeAdminController : Controller
    {
        private readonly ICraftService _craftService;
        public CraftTypeAdminController(ICraftService craftService)
        {
            _craftService = craftService;
        }

        public async Task<IActionResult> Index()
        {
            var types = await _craftService.GetCraftTypesAsync();
            return View(types);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CraftType model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                model.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.Name.ToLower());
            }

            await _craftService.AddCraftTypeAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var type = await _craftService.GetCraftTypeByIdAsync(id.Value);
            if (type == null) return NotFound();
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CraftType model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var type = await _craftService.GetCraftTypeByIdAsync(id);
            if (type == null) return NotFound();
            type.Name = model.Name;
            await _craftService.UpdateCraftTypeAsync(type);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var type = await _craftService.GetCraftTypeByIdAsync(id.Value);
            if (type == null) return NotFound();
            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _craftService.SoftDeleteCraftTypeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
