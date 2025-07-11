using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;

namespace smelite_app.Repositories
{
    public class MasterRepository : IMasterRepository
    {
        private readonly ApplicationDbContext _context;
        public MasterRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddProfileAsync(MasterProfile profile)
        {
            _context.MasterProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public Task<MasterProfile?> GetByUserIdAsync(string userId)
        {
            return _context.MasterProfiles
                .Include(mp => mp.ApplicationUser)
                .Include(mp => mp.MasterProfileCrafts)
                    .ThenInclude(mpc => mpc.Craft)
                .Include(mp => mp.Tasks)
                    .ThenInclude(t => t.ApprenticeProfile)
                        .ThenInclude(ap => ap.ApplicationUser)
                .FirstOrDefaultAsync(mp => mp.ApplicationUserId == userId);
        }

        public async Task UpdateProfileAsync(MasterProfile profile)
        {
            _context.MasterProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task AddCraftAsync(int masterProfileId, Craft craft)
        {
            _context.Crafts.Add(craft);
            await _context.SaveChangesAsync();

            _context.MasterProfileCrafts.Add(new MasterProfileCraft
            {
                MasterProfileId = masterProfileId,
                CraftId = craft.Id
            });
            await _context.SaveChangesAsync();
        }

        public Task<List<Craft>> GetCraftsAsync(int masterProfileId)
        {
            return _context.MasterProfileCrafts
                .Where(mpc => mpc.MasterProfileId == masterProfileId)
                .Select(mpc => mpc.Craft)
                .Include(c => c.CraftType)
                .ToListAsync();
        }

        public Task<List<Apprenticeship>> GetApprenticeshipsAsync(int masterProfileId)
        {
            return _context.Apprenticeships
                .Include(a => a.ApprenticeProfile)
                    .ThenInclude(ap => ap.ApplicationUser)
                .Include(a => a.CraftOffering)
                    .ThenInclude(o => o.Craft)
                .Where(a => a.MasterProfileId == masterProfileId)
                .ToListAsync();
        }

        public Task<Craft?> GetCraftByIdAsync(int craftId)
        {
            return _context.Crafts
                .Include(c => c.MasterProfileCrafts)
                .FirstOrDefaultAsync(c => c.Id == craftId);
        }

        public async Task UpdateCraftAsync(Craft craft)
        {
            _context.Crafts.Update(craft);
            await _context.SaveChangesAsync();
        }
    }
}
