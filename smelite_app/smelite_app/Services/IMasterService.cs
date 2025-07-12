using smelite_app.Models;
using smelite_app.ViewModels.Master;

namespace smelite_app.Services
{
    public interface IMasterService
    {
        Task<MasterProfileViewModel?> GetProfileAsync(string userId);
        Task<MasterProfile?> GetByUserIdAsync(string userId);
        Task UpdateProfileAsync(MasterProfile profile);

        Task AddCraftAsync(int masterProfileId, CraftViewModel model, string webRootPath, string userId);
        Task<List<Craft>> GetCraftsAsync(int masterProfileId);
        Task<Craft?> GetCraftByIdAsync(int craftId);
        Task UpdateCraftAsync(EditCraftViewModel model, int masterProfileId, string webRootPath, string userId);
        Task<List<Apprenticeship>> GetApprenticeshipsAsync(int masterProfileId);
    }
}
