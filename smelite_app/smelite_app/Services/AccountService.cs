using Microsoft.AspNetCore.Identity;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.ViewModels.Account;

namespace smelite_app.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMasterRepository _masterRepo;
        private readonly IApprenticeRepository _apprenticeRepo;
        private readonly ILogger<MasterRepository> _logger;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMasterRepository masterRepo,
            IApprenticeRepository apprenticeRepo,
            ILogger<MasterRepository> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _masterRepo = masterRepo;
            _apprenticeRepo = apprenticeRepo;
            _logger = logger;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            _logger.LogInformation("Login attempt with email {Email}", model.Email);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt with non-existing email {Email}", model.Email);
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
                _logger.LogInformation("Successful login for {Email}", model.Email);
            else
                _logger.LogWarning("Failed login attempt for {Email}", model.Email);

            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            _logger.LogInformation("Registration attempt with email {Email}", model.Email);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User already exists with email {Email}", model.Email);
                return IdentityResult.Failed(new IdentityError { Description = "A user with this email already exists." });
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = model.Role,
                ProfileImageUrl = Helpers.Variables.defaultProfileImageUrl
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Automatically confirm the email
                await _userManager.ConfirmEmailAsync(user,
                    await _userManager.GenerateEmailConfirmationTokenAsync(user));

                await _userManager.AddToRoleAsync(user, model.Role);

                if (model.Role == "Master")
                    await _masterRepo.AddProfileAsync(new MasterProfile { ApplicationUserId = user.Id });
                else if (model.Role == "Apprentice")
                    await _apprenticeRepo.AddProfileAsync(new ApprenticeProfile { ApplicationUserId = user.Id });

                _logger.LogInformation("Successful registration for {Email}", model.Email);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Registration error: {Error}", error.Description);
                }
            }
            return result;
        }
    }
}
