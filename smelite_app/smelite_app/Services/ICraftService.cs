using smelite_app.Models;

namespace smelite_app.Services
{
    public interface ICraftService
    {
        Task<IEnumerable<MasterProfile>> GetFilteredMastersAsync(int? craftTypeId, int? locationId, string? searchName);
        Task<MasterProfile?> GetByIdAsync(int id);
        Task<List<CraftType>> GetCraftTypesAsync();
        Task<List<CraftLocation>> GetLocationsAsync();

    }
}
