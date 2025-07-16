using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using smelite_app.Models;
using smelite_app.Services;
using smelite_app.ViewModels.Master;
using smelite_app.Helpers;

namespace smelite_app.Controllers
{
    [Authorize(Roles = "Master")]
    public class MasterController : Controller
    {
        private readonly IMasterService _masterService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICraftService _craftService;
        private readonly IWebHostEnvironment _environment;

        public MasterController(IMasterService masterService, UserManager<ApplicationUser> userManager, ICraftService craftService, IWebHostEnvironment environment)
        {
            _masterService = masterService;
            _userManager = userManager;
            _craftService = craftService;
            _environment = environment;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var profile = await _masterService.GetProfileAsync(user.Id);
            if (profile == null) return NotFound();
            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = await _masterService.GetByUserIdAsync(user.Id);
            if (profile == null) return NotFound();

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword!);
            if (!passwordValid)
            {
                ModelState.AddModelError("CurrentPassword", "Invalid password.");
                return View(model);
            }

            var vm = new EditMasterProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PersonalInformation = profile.PersonalInformation,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditMasterProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = await _masterService.GetByUserIdAsync(user.Id);
            if (profile == null) return NotFound();

            bool userChanged = false;
            if (user.FirstName != model.FirstName)
            {
                user.FirstName = model.FirstName;
                userChanged = true;
            }
            if (user.LastName != model.LastName)
            {
                user.LastName = model.LastName;
                userChanged = true;
            }
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                userChanged = true;
            }
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "ProfileImages");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }
                model.ProfileImageUrl = "/ProfileImages/" + fileName;
            }
            if (user.ProfileImageUrl != model.ProfileImageUrl)
            {
                user.ProfileImageUrl = model.ProfileImageUrl;
                userChanged = true;
            }

            if (userChanged)
                await _userManager.UpdateAsync(user);

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword!, model.NewPassword!);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            profile.PersonalInformation = model.PersonalInformation;
            await _masterService.UpdateProfileAsync(profile);

            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public async Task<IActionResult> CreateCraft()
        {
            var types = await _craftService.GetCraftTypesAsync();
            var vm = new CraftViewModel
            {
                CraftTypes = new SelectList(types, "Id", "Name"),
                Offerings = new List<CraftOfferingFormViewModel> { new CraftOfferingFormViewModel() }
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCraft(CraftViewModel craft)
        {
            if (!ModelState.IsValid)
            {
                craft.CraftTypes = new SelectList(await _craftService.GetCraftTypesAsync(), "Id", "Name");
                return View(craft);
            }

            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            await _masterService.AddCraftAsync(profile.Id, craft, _environment.WebRootPath, user.Id);
            return RedirectToAction(nameof(Crafts));
        }

        public async Task<IActionResult> Crafts()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var crafts = await _masterService.GetCraftsAsync(profile.Id);
            var vm = crafts.Select(c => new MasterCraftListItemViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.CraftType.Name
            }).ToList();
            return View(vm);
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

            var types = await _craftService.GetCraftTypesAsync();
            var vm = new EditCraftViewModel
            {
                Id = craft.Id,
                Name = craft.Name,
                CraftDescription = craft.CraftDescription,
                ExperienceYears = craft.ExperienceYears,
                CraftTypeId = craft.CraftTypeId,
                CraftTypes = new SelectList(types, "Id", "Name", craft.CraftTypeId),
                Offerings = craft.CraftOfferings.Select(o => new CraftOfferingFormViewModel
                {
                    Id = o.Id,
                    LocationName = o.CraftLocation.Name,
                    SessionsCount = o.CraftPackage.SessionsCount,
                    PackageLabel = o.CraftPackage.Label,
                    Price = o.Price
                }).ToList(),
                ExistingImages = craft.Images.Select(i => new CraftImageItemViewModel
                {
                    Id = i.Id,
                    Url = i.ImageUrl
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCraft(EditCraftViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CraftTypes = new SelectList(await _craftService.GetCraftTypesAsync(), "Id", "Name", model.CraftTypeId);
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            await _masterService.UpdateCraftAsync(model, profile.Id, _environment.WebRootPath, user.Id);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCraft(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var craft = await _masterService.GetCraftByIdAsync(id);
            if (craft == null || !craft.MasterProfileCrafts.Any(m => m.MasterProfileId == profile.Id))
                return NotFound();

            await _craftService.SoftDeleteCraftAsync(id);
            return RedirectToAction(nameof(Crafts));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCraftOffering(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var offering = await _craftService.GetCraftOfferingByIdAsync(id);
            if (offering == null || !offering.Craft.MasterProfileCrafts.Any(m => m.MasterProfileId == profile.Id))
                return NotFound();

            await _craftService.SoftDeleteCraftOfferingAsync(id);
            return RedirectToAction(nameof(EditCraft), new { id = offering.CraftId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _masterService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var image = await _craftService.GetCraftImageByIdAsync(id);
            if (image == null || !image.Craft.MasterProfileCrafts.Any(m => m.MasterProfileId == profile.Id))
                return NotFound();

            if (!string.IsNullOrEmpty(image.ImageUrl) && !image.ImageUrl.Equals(Variables.defaultCraftImageUrl, StringComparison.OrdinalIgnoreCase))
            {
                var path = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            await _craftService.RemoveCraftImageAsync(id);
            return RedirectToAction(nameof(EditCraft), new { id = image.CraftId });
        }
    }
}
