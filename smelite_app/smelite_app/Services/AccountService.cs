using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.ViewModels.Account;
using Microsoft.AspNetCore.Mvc.Routing;
using smelite_app.Helpers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace smelite_app.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMasterRepository _masterRepo;
        private readonly IApprenticeRepository _apprenticeRepo;
        private readonly ILogger<AccountService> _logger;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailSender _emailSender;

        public AccountService(
            IAccountRepository accountRepo,
            IMasterRepository masterRepo,
            IApprenticeRepository apprenticeRepo,
            ILogger<AccountService> logger,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor,
            EmailSender emailSender)
        {
            _accountRepo = accountRepo;
            _masterRepo = masterRepo;
            _apprenticeRepo = apprenticeRepo;
            _logger = logger;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            _logger.LogInformation("Login attempt with email {Email}", model.Email);

            var user = await _accountRepo.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt with non-existing email {Email}", model.Email);
                return SignInResult.Failed;
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Login attempt for unconfirmed email {Email}", model.Email);
                return SignInResult.NotAllowed;
            }

            if (user.Role != "Admin")
            {
                bool isActive = true;

                if (user.Role == "Master")
                {
                    var profile = await _masterRepo.GetByUserIdAsync(user.Id);
                    isActive = profile?.IsActive ?? false;
                }
                else if (user.Role == "Apprentice")
                {
                    var profile = await _apprenticeRepo.GetByUserIdAsync(user.Id);
                    isActive = profile?.IsActive ?? false;
                }

                if (!isActive)
                {
                    _logger.LogWarning("Login attempt for deactivated user {Email}", model.Email);
                    return SignInResult.NotAllowed;
                }
            }

            var result = await _accountRepo.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
                _logger.LogInformation("Successful login for {Email}", model.Email);
            else
                _logger.LogWarning("Failed login attempt for {Email}", model.Email);

            return result;
        }

        public async Task LogoutAsync()
        {
            await _accountRepo.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            _logger.LogInformation("Registration attempt with email {Email}", model.Email);

            var existingUser = await _accountRepo.FindByEmailAsync(model.Email);
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

            var result = await _accountRepo.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _accountRepo.AddToRoleAsync(user, model.Role);

                if (model.Role == "Master")
                    await _masterRepo.AddProfileAsync(new MasterProfile { ApplicationUserId = user.Id });
                else if (model.Role == "Apprentice")
                    await _apprenticeRepo.AddProfileAsync(new ApprenticeProfile { ApplicationUserId = user.Id });

                var token = await _accountRepo.GenerateEmailConfirmationTokenAsync(user);

                var httpContext = _httpContextAccessor.HttpContext!;
                var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor()));
                var link = urlHelper.Action("ConfirmEmail", "Account", new { userId = user.Id, code = token }, httpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email!, "Confirm your email", $"Please confirm your account by <a href='{link}'>clicking here</a>.");

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

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _accountRepo.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Email confirmation failed for missing user {UserId}", userId);
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await _accountRepo.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                _logger.LogInformation("Email confirmed for {Email}", user.Email);
            else
                foreach (var error in result.Errors)
                    _logger.LogWarning("Email confirmation error: {Error}", error.Description);

            return result;
        }

    }
}
