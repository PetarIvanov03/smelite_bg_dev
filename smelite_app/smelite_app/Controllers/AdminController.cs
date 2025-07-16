using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using smelite_app.Models;
using smelite_app.Services;
using smelite_app.ViewModels.Admin;

namespace smelite_app.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _adminService = adminService;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            var vm = new AdminProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            var vm = new EditAdminProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditAdminProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword!);
            if (!passwordValid)
            {
                ModelState.AddModelError("CurrentPassword", "Invalid password.");
                return View(model);
            }

            bool changed = false;
            if (user.FirstName != model.FirstName) { user.FirstName = model.FirstName; changed = true; }
            if (user.LastName != model.LastName) { user.LastName = model.LastName; changed = true; }
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                changed = true;
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
                changed = true;
            }
            if (changed)
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
            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> Users()
        {
            var users = await _adminService.GetUsersAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUser(string id)
        {
            var current = await _userManager.GetUserAsync(User);
            if (current == null || current.Id == id) return BadRequest();
            var users = await _adminService.GetUsersAsync();
            var target = users.FirstOrDefault(u => u.Id == id);
            if (target == null) return NotFound();
            bool active = !(target.MasterProfile?.IsActive ?? target.ApprenticeProfile?.IsActive ?? true);
            await _adminService.ToggleUserActivationAsync(id, active);
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> Apprenticeships()
        {
            var apps = await _adminService.GetApprenticeshipsAsync();
            return View(apps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetApprenticeshipStatus(int id, string status)
        {
            await _adminService.UpdateApprenticeshipStatusAsync(id, status);
            return RedirectToAction(nameof(Apprenticeships));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPaymentStatus(int id, string status)
        {
            await _adminService.UpdatePaymentStatusAsync(id, status);
            return RedirectToAction(nameof(Orders));
        }

        public async Task<IActionResult> Orders()
        {
            var payments = await _adminService.GetPaymentsAsync();
            return View(payments);
        }
    }
}
