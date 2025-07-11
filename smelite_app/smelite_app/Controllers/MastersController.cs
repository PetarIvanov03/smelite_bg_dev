using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smelite_app.Services;
using smelite_app.ViewModels.Master;

namespace smelite_app.Controllers
{
    public class MastersController : Controller
    {
        private readonly IMasterService _masterService;
        public MastersController(IMasterService masterService)
        {
            _masterService = masterService;
        }

        public async Task<IActionResult> Index(int? craftTypeId, int? locationId, string? search)
        {
            var masters = await _masterService.GetFilteredMastersAsync(craftTypeId, locationId, search);
            var items = masters.Select(m => new MasterListItemViewModel
            {
                Id = m.Id,
                Name = m.ApplicationUser.FirstName + " " + m.ApplicationUser.LastName,
                PersonalInfo = m.PersonalInformation,
                PhotoUrl = m.ApplicationUser.ProfileImageUrl,
                Crafts = m.MasterProfileCrafts.Select(c => c.Craft.Name).ToList()
            }).ToList();

            var vm = new MasterIndexViewModel
            {
                Masters = items,
                CraftTypeId = craftTypeId,
                LocationId = locationId,
                SearchString = search,
                CraftTypes = new SelectList(await _masterService.GetCraftTypesAsync(), "Id", "Name"),
                Locations = new SelectList(await _masterService.GetLocationsAsync(), "Id", "Name")
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var master = await _masterService.GetByIdAsync(id);
            if (master == null)
                return NotFound();

            var vm = new MasterDetailsViewModel
            {
                Id = master.Id,
                Name = master.ApplicationUser.FirstName + " " + master.ApplicationUser.LastName,
                PersonalInfo = master.PersonalInformation,
                PhotoUrl = master.ApplicationUser.ProfileImageUrl,
                Crafts = master.MasterProfileCrafts.Select(c => c.Craft.Name).ToList()
            };
            return View(vm);
        }
    }
}
