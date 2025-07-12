using smelite_app.Models;

namespace smelite_app.Services
{
    public interface ICraftService
    {
        Task<IEnumerable<MasterProfile>> GetFilteredMastersAsync(int? craftTypeId, int? locationId, string? searchName);
        Task<MasterProfile?> GetByIdAsync(int id);

        Task<IEnumerable<Craft>> GetFilteredCraftsAsync(int? craftTypeId, int? locationId, string? searchName);
        Task<Craft?> GetCraftByIdAsync(int id);
        Task<List<CraftType>> GetCraftTypesAsync();
        Task<List<CraftLocation>> GetLocationsAsync();
        Task<List<CraftPackage>> GetPackagesAsync();

        Task<CraftOffering?> GetCraftOfferingByIdAsync(int id);
        Task<CraftImage?> GetCraftImageByIdAsync(int id);

        Task SoftDeleteCraftAsync(int craftId);
        Task SoftDeleteCraftTypeAsync(int craftTypeId);
        Task SoftDeleteCraftOfferingAsync(int offeringId);
        Task RemoveCraftImageAsync(int imageId);
    }
}
