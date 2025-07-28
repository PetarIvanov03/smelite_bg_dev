using Microsoft.EntityFrameworkCore;
using smelite_app.Models;

namespace smelite_app.Repositories
{
    public interface ICraftRepository
    {
        IQueryable<MasterProfile> GetAll();
        Task<MasterProfile?> GetByIdAsync(int id);

        IQueryable<Craft> GetCrafts();
        Task<Craft?> GetCraftByIdAsync(int craftId);
        Task<List<CraftType>> GetCraftTypesAsync();
        Task<CraftType?> GetCraftTypeByIdAsync(int craftTypeId);
        Task AddCraftTypeAsync(CraftType craftType);
        Task UpdateCraftTypeAsync(CraftType craftType);

        Task<List<CraftLocation>> GetLocationsAsync();
        Task<List<CraftPackage>> GetPackagesAsync();

        Task<CraftOffering?> GetCraftOfferingByIdAsync(int offeringId);
        Task<CraftImage?> GetCraftImageByIdAsync(int imageId);

        Task SoftDeleteCraftAsync(int craftId);
        Task SoftDeleteCraftTypeAsync(int craftTypeId);
        Task SoftDeleteCraftOfferingAsync(int offeringId);
        Task RemoveCraftImageAsync(int imageId);
    }
}
