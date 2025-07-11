using smelite_app.Models;

namespace smelite_app.Services
{
    public interface IMasterService
    {
        Task<IEnumerable<MasterProfile>> GetFilteredMastersAsync(int? craftTypeId, int? locationId, string? searchName);
        Task<MasterProfile?> GetByIdAsync(int id);
        Task<List<CraftType>> GetCraftTypesAsync();
        Task<List<CraftLocation>> GetLocationsAsync();

        Task<MasterProfile?> GetByUserIdAsync(string userId);
        Task UpdateProfileAsync(MasterProfile profile);
        Task AddCraftAsync(int masterProfileId, Craft craft);
        Task<List<Craft>> GetCraftsAsync(int masterProfileId);
        Task<List<Apprenticeship>> GetApprenticeshipsAsync(int masterProfileId);
    }
}
