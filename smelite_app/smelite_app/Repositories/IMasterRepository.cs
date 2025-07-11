using smelite_app.Models;

namespace smelite_app.Repositories
{
    public interface IMasterRepository
    {
        Task AddProfileAsync(MasterProfile profile);

        Task<MasterProfile?> GetByUserIdAsync(string userId);
        Task UpdateProfileAsync(MasterProfile profile);

        Task AddCraftAsync(int masterProfileId, Craft craft);
        Task<List<Craft>> GetCraftsAsync(int masterProfileId);

        Task<List<Apprenticeship>> GetApprenticeshipsAsync(int masterProfileId);
    }
}
