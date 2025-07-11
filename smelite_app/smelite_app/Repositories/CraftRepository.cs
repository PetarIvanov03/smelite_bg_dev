using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;

namespace smelite_app.Repositories
{
    public class CraftRepository : ICraftRepository
    {
        private readonly ApplicationDbContext _context;
        public CraftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<MasterProfile> GetAll()
        {
            return _context.MasterProfiles
                .Include(m => m.ApplicationUser)
                .Include(m => m.MasterProfileCrafts)
                    .ThenInclude(mpc => mpc.Craft)
                        .ThenInclude(c => c.CraftOfferings)
                            .ThenInclude(o => o.CraftLocation)
                .Include(m => m.MasterProfileCrafts)
                    .ThenInclude(mpc => mpc.Craft)
                        .ThenInclude(c => c.CraftOfferings)
                            .ThenInclude(o => o.CraftPackage)
                .Include(m => m.MasterProfileCrafts)
                    .ThenInclude(mpc => mpc.Craft)
                        .ThenInclude(c => c.CraftType);
        }

        public async Task<MasterProfile?> GetByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(m => m.Id == id);
        }

        public Task<List<CraftType>> GetCraftTypesAsync()
        {
            return _context.CraftTypes.ToListAsync();
        }

        public Task<List<CraftLocation>> GetLocationsAsync()
        {
            return _context.CraftLocations.ToListAsync();
        }

        public Task<List<CraftPackage>> GetPackagesAsync()
        {
            return _context.CraftPackages.ToListAsync();
        }
    }
}
