using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smelite_app.Models;
using smelite_app.Services;
using smelite_app.ViewModels.Master;

namespace smelite_app.Controllers
{
    [Authorize(Roles = "Master")]
    public class MasterController : Controller
    {
        private readonly IMasterService _masterService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MasterController(IMasterService masterService, UserManager<ApplicationUser> userManager)
        {
            _masterService = masterService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var profile = await _masterService.GetByUserIdAsync(user.Id);
            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = await _masterService.GetByUserIdAsync(user.Id);
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(MasterProfile model)
        {
            if (!ModelState.IsValid) return View(model);
            await _masterService.UpdateProfileAsync(model);
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult CreateCraft()
        {
            return View(new CraftViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCraft(CraftViewModel craft)
        {
            if (!ModelState.IsValid) return View(craft);

            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var entity = new Craft
            {
                Name = craft.Name,
                CraftDescription = craft.CraftDescription,
                ExperienceYears = craft.ExperienceYears
            };
            await _masterService.AddCraftAsync(profile.Id, entity);
            return RedirectToAction(nameof(Crafts));
        }

        public async Task<IActionResult> Crafts()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var crafts = await _masterService.GetCraftsAsync(profile.Id);
            return View(crafts);
        }

        [HttpGet]
        public async Task<IActionResult> EditCraft(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var craft = await _masterService.GetCraftByIdAsync(id);
            if (craft == null || !craft.MasterProfileCrafts.Any(m => m.MasterProfileId == profile.Id))
                return NotFound();

            var vm = new EditCraftViewModel
            {
                Id = craft.Id,
                Name = craft.Name,
                CraftDescription = craft.CraftDescription,
                ExperienceYears = craft.ExperienceYears
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCraft(EditCraftViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var craft = await _masterService.GetCraftByIdAsync(model.Id);
            if (craft == null || !craft.MasterProfileCrafts.Any(m => m.MasterProfileId == profile.Id))
                return NotFound();

            craft.Name = model.Name;
            craft.CraftDescription = model.CraftDescription;
            craft.ExperienceYears = model.ExperienceYears;
            await _masterService.UpdateCraftAsync(craft);

            return RedirectToAction(nameof(Crafts));
        }

        public async Task<IActionResult> Sessions()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var apps = await _masterService.GetApprenticeshipsAsync(profile.Id);
            return View(apps);
        }
    }
}
