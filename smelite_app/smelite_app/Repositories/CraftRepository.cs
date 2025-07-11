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

        public IQueryable<Craft> GetCrafts()
        {
            return _context.Crafts
                .Include(c => c.CraftType)
                .Include(c => c.Images)
                .Include(c => c.CraftOfferings)
                    .ThenInclude(o => o.CraftLocation)
                .Include(c => c.CraftOfferings)
                    .ThenInclude(o => o.CraftPackage)
                .Include(c => c.MasterProfileCrafts)
                    .ThenInclude(mpc => mpc.MasterProfile)
                        .ThenInclude(mp => mp.ApplicationUser);
        }

        public Task<Craft?> GetCraftByIdAsync(int craftId)
        {
            return GetCrafts().FirstOrDefaultAsync(c => c.Id == craftId);
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

        public async Task SoftDeleteCraftAsync(int craftId)
        {
            var craft = await _context.Crafts.FindAsync(craftId);
            if (craft != null)
            {
                craft.IsDeleted = true;
                var offerings = _context.CraftOfferings.Where(o => o.CraftId == craftId);
                await offerings.ForEachAsync(o => o.IsDeleted = true);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SoftDeleteCraftTypeAsync(int craftTypeId)
        {
            var type = await _context.CraftTypes.FindAsync(craftTypeId);
            if (type != null)
            {
                type.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SoftDeleteCraftOfferingAsync(int offeringId)
        {
            var offering = await _context.CraftOfferings.FindAsync(offeringId);
            if (offering != null)
            {
                offering.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
