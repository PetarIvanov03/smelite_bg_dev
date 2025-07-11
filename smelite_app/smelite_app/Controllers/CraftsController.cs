using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smelite_app.Services;
using smelite_app.ViewModels.Master;

namespace smelite_app.Controllers
{
    public class CraftsController : Controller
    {
        private readonly ICraftService _craftService;
        public CraftsController(ICraftService craftService)
        {
            _craftService = craftService;
        }

        public async Task<IActionResult> Index(int? craftTypeId, int? locationId, string? search)
        {
            var masters = await _craftService.GetFilteredMastersAsync(craftTypeId, locationId, search);
            var items = masters.Select(m => new CraftListItemViewModel
            {
                Id = m.Id,
                Name = m.ApplicationUser.FirstName + " " + m.ApplicationUser.LastName,
                PersonalInfo = m.PersonalInformation,
                PhotoUrl = m.ApplicationUser.ProfileImageUrl,
                Crafts = m.MasterProfileCrafts.Select(c => c.Craft.Name).ToList()
            }).ToList();

            var vm = new CraftIndexViewModel
            {
                Masters = items,
                CraftTypeId = craftTypeId,
                LocationId = locationId,
                SearchString = search,
                CraftTypes = new SelectList(await _craftService.GetCraftTypesAsync(), "Id", "Name"),
                Locations = new SelectList(await _craftService.GetLocationsAsync(), "Id", "Name")
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var master = await _craftService.GetByIdAsync(id);
            if (master == null)
                return NotFound();

            var vm = new CraftDetailsViewModel
            {
                Id = master.Id,
                Name = master.ApplicationUser.FirstName + " " + master.ApplicationUser.LastName,
                PersonalInfo = master.PersonalInformation,
                PhotoUrl = master.ApplicationUser.ProfileImageUrl,
                Crafts = master.MasterProfileCrafts.Select(c => c.Craft.Name).ToList(),
                Offerings = master.MasterProfileCrafts
                    .SelectMany(c => c.Craft.CraftOfferings)
                    .Select(o => new CraftOfferingDetailsViewModel
                    {
                        Id = o.Id,
                        Location = o.CraftLocation.Name,
                        Package = o.CraftPackage.Label ?? o.CraftPackage.SessionsCount.ToString(),
                        Price = o.Price
                    }).ToList()
            };
            return View(vm);
        }
    }
}
