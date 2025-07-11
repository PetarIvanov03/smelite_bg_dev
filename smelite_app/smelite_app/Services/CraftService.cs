using Microsoft.EntityFrameworkCore;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Data;

namespace smelite_app.Services
{
    public class CraftService : ICraftService
    {
        private readonly ICraftRepository _repository;

        public CraftService(ICraftRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MasterProfile>> GetFilteredMastersAsync(int? craftTypeId, int? locationId, string? searchName)
        {
            var query = _repository.GetAll();

            if (craftTypeId.HasValue)
            {
                query = query.Where(m => m.MasterProfileCrafts.Any(c => c.Craft.CraftTypeId == craftTypeId.Value));
            }

            if (locationId.HasValue)
            {
                query = query.Where(m => m.MasterProfileCrafts.Any(c => c.Craft.CraftOfferings.Any(o => o.CraftLocationId == locationId.Value)));
            }

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(m => (m.ApplicationUser.FirstName + " " + m.ApplicationUser.LastName).Contains(searchName));
            }

            return await query.ToListAsync();
        }

        public Task<MasterProfile?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Craft>> GetFilteredCraftsAsync(int? craftTypeId, int? locationId, string? searchName)
        {
            var query = _repository.GetCrafts();

            if (craftTypeId.HasValue)
                query = query.Where(c => c.CraftTypeId == craftTypeId.Value);

            if (locationId.HasValue)
                query = query.Where(c => c.CraftOfferings.Any(o => o.CraftLocationId == locationId.Value));

            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(c => c.Name.Contains(searchName));

            return await query.ToListAsync();
        }

        public Task<Craft?> GetCraftByIdAsync(int id)
        {
            return _repository.GetCraftByIdAsync(id);
        }

        public Task<List<CraftType>> GetCraftTypesAsync()
        {
            return _repository.GetCraftTypesAsync();
        }

        public Task<List<CraftLocation>> GetLocationsAsync()
        {
            return _repository.GetLocationsAsync();
        }

        public Task<List<CraftPackage>> GetPackagesAsync()
        {
            return _repository.GetPackagesAsync();
        }
    }
}
