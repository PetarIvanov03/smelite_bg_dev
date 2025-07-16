using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using smelite_app.Models;
using smelite_app.Services;
using smelite_app.ViewModels.Apprentice;

namespace smelite_app.Controllers
{
    [Authorize(Roles = "Apprentice")]
    public class ApprenticeController : Controller
    {
        private readonly IApprenticeService _apprenticeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ApprenticeController(IApprenticeService apprenticeService, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _apprenticeService = apprenticeService;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = await _apprenticeService.GetProfileAsync(user.Id);
            if (profile == null) return NotFound();

            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = await _apprenticeService.GetByUserIdAsync(user.Id);
            if (profile == null) return NotFound();

            var vm = new EditApprenticeProfileViewModel
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
        public async Task<IActionResult> EditProfile(EditApprenticeProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = await _apprenticeService.GetByUserIdAsync(user.Id);
            if (profile == null) return NotFound();

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword!);
            if (!passwordValid)
            {
                ModelState.AddModelError("CurrentPassword", "Invalid password.");
                return View(model);
            }

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
            await _apprenticeService.UpdateProfileAsync(profile);

            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> Sessions()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _apprenticeService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            var apps = await _apprenticeService.GetApprenticeshipsAsync(profile.Id);
            return View(apps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(int offeringId)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _apprenticeService.GetByUserIdAsync(user!.Id);
            if (profile == null) return NotFound();

            await _apprenticeService.AddApprenticeshipAsync(profile.Id, offeringId);
            return RedirectToAction(nameof(Sessions));
        }
    }
}
