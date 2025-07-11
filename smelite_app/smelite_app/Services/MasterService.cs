using Microsoft.EntityFrameworkCore;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Data;

namespace smelite_app.Services
{
    public class MasterService : IMasterService
    {
        private readonly ICraftRepository _repository;
        private readonly ApplicationDbContext _context;

        public MasterService(ICraftRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
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

        public Task<List<CraftType>> GetCraftTypesAsync()
        {
            return _context.CraftTypes.ToListAsync();
        }

        public Task<List<CraftLocation>> GetLocationsAsync()
        {
            return _context.CraftLocations.ToListAsync();
        }
    }
}
