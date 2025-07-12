using smelite_app.Models;
using smelite_app.ViewModels.Apprentice;

namespace smelite_app.Services
{
    public interface IApprenticeService
    {
        Task<ApprenticeProfileViewModel?> GetProfileAsync(string userId);
        Task<ApprenticeProfile?> GetByUserIdAsync(string userId);
        Task UpdateProfileAsync(ApprenticeProfile profile);

        Task AddApprenticeshipAsync(int apprenticeProfileId, int craftOfferingId);
        Task<List<Apprenticeship>> GetApprenticeshipsAsync(int apprenticeProfileId);
    }
}
