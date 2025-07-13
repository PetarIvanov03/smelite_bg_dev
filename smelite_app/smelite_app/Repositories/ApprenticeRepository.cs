using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;

namespace smelite_app.Repositories
{
    public class ApprenticeRepository : IApprenticeRepository
    {
        private readonly ApplicationDbContext _context;
        public ApprenticeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProfileAsync(ApprenticeProfile profile)
        {
            _context.ApprenticeProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public Task<ApprenticeProfile?> GetByUserIdAsync(string userId)
        {
            return _context.ApprenticeProfiles
                .Include(ap => ap.ApplicationUser)
                .FirstOrDefaultAsync(ap => ap.ApplicationUserId == userId);
        }

        public async Task UpdateProfileAsync(ApprenticeProfile profile)
        {
            _context.ApprenticeProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task AddApprenticeshipAsync(Apprenticeship apprenticeship)
        {
            _context.Apprenticeships.Add(apprenticeship);
            await _context.SaveChangesAsync();
        }

        public Task<List<Apprenticeship>> GetApprenticeshipsAsync(int apprenticeProfileId)
        {
            return _context.Apprenticeships
                .Include(a => a.MasterProfile)
                    .ThenInclude(mp => mp.ApplicationUser)
                .Include(a => a.CraftOffering)
                    .ThenInclude(o => o.Craft)
                .Where(a => a.ApprenticeProfileId == apprenticeProfileId)
                .IgnoreQueryFilters()
                .ToListAsync();
        }
    }
}
