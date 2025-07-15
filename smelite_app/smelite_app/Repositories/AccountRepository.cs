using Microsoft.AspNetCore.Identity;
using smelite_app.Models;

namespace smelite_app.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountRepository(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            return _userManager.FindByIdAsync(userId);
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task AddToRoleAsync(ApplicationUser user, string role)
        {
            return _userManager.AddToRoleAsync(user, role);
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public Task ConfirmEmailAsync(ApplicationUser user, string token)
        {
            return _userManager.ConfirmEmailAsync(user, token);
        }

        public Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);
        }

        public Task SignOutAsync()
        {
            return _signInManager.SignOutAsync();
        }

        public Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            return _signInManager.SignInAsync(user, isPersistent);
        }
    }
}
