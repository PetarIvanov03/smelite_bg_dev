using smelite_app.Models;

namespace smelite_app.Services
{
    public interface IAdminService
    {
        Task<List<ApplicationUser>> GetUsersAsync();
        Task ToggleUserActivationAsync(string userId, bool isActive);
        Task<List<Apprenticeship>> GetApprenticeshipsAsync();
        Task UpdateApprenticeshipStatusAsync(int id, string status);
        Task<List<Payment>> GetPaymentsAsync();
    }
}
