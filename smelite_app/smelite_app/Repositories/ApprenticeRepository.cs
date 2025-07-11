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
    }
}
