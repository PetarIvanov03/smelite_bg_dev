using smelite_app.Models;

namespace smelite_app.Repositories
{
    public interface IApprenticeRepository
    {
        Task AddProfileAsync(ApprenticeProfile profile);
        Task<ApprenticeProfile?> GetByUserIdAsync(string userId);
        Task UpdateProfileAsync(ApprenticeProfile profile);

        Task AddApprenticeshipAsync(Apprenticeship apprenticeship);
        Task<List<Apprenticeship>> GetApprenticeshipsAsync(int apprenticeProfileId);
    }
}
