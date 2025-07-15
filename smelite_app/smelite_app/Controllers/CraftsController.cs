using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using smelite_app.Helpers;
using smelite_app.Services;
using smelite_app.ViewModels.Craft;

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
            var sender = new EmailSender();
            bool success = await sender.SendEmailAsync("i.petarivanov03@gmail.com", "Test", "Test message");
            Console.WriteLine(success ? "Sent!" : "Failed!");

            var crafts = await _craftService.GetFilteredCraftsAsync(craftTypeId, locationId, search);
            var items = crafts.Select(c => new CraftListItemViewModel
            {
                Id = c.Id,
                Type = c.CraftType.Name,
                Name = c.Name,
                MasterName = c.MasterProfileCrafts
                    .Select(m => m.MasterProfile.ApplicationUser.FirstName + " " + m.MasterProfile.ApplicationUser.LastName)
                    .FirstOrDefault() ?? string.Empty
            }).ToList();

            var vm = new CraftIndexViewModel
            {
                Crafts = items,
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
            var craft = await _craftService.GetCraftByIdAsync(id);
            if (craft == null)
                return NotFound();

            var vm = new CraftDetailsViewModel
            {
                Id = craft.Id,
                Type = craft.CraftType.Name,
                Name = craft.Name,
                ImageUrls = craft.Images.Select(i => i.ImageUrl).ToList(),
                Description = craft.CraftDescription,
                Offerings = craft.CraftOfferings.Select(o => new CraftOfferingDetailsViewModel
                {
                    Id = o.Id,
                    Location = o.CraftLocation.Name,
                    Package = o.CraftPackage.Label ?? o.CraftPackage.SessionsCount.ToString(),
                    SessionsCount = o.CraftPackage.SessionsCount,
                    Price = o.Price
                }).ToList(),
                MasterName = craft.MasterProfileCrafts
                    .Select(m => m.MasterProfile.ApplicationUser.FirstName + " " + m.MasterProfile.ApplicationUser.LastName)
                    .FirstOrDefault() ?? string.Empty,
                MasterInfo = craft.MasterProfileCrafts.Select(m => m.MasterProfile.PersonalInformation).FirstOrDefault(),
                MasterPhotoUrl = craft.MasterProfileCrafts.Select(m => m.MasterProfile.ApplicationUser.ProfileImageUrl).FirstOrDefault()
            };
            return View(vm);
        }
    }
}
