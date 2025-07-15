using Microsoft.AspNetCore.Identity;
using smelite_app.ViewModels.Account;

namespace smelite_app.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
    }
}
