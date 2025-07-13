using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;
using smelite_app.Enums;

namespace smelite_app.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<ApplicationUser>> GetUsersAsync()
        {
            return _context.Users
                .Include(u => u.ApprenticeProfile)
                .Include(u => u.MasterProfile)
                .Where(u => u.Role != "Admin")
                .ToListAsync();
        }

        public async Task ToggleUserActivationAsync(string userId, bool isActive)
        {
            var user = await _context.Users
                .Include(u => u.ApprenticeProfile)
                .Include(u => u.MasterProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return;
            if (user.ApprenticeProfile != null)
                user.ApprenticeProfile.IsActive = isActive;
            if (user.MasterProfile != null)
                user.MasterProfile.IsActive = isActive;
            await _context.SaveChangesAsync();
        }

        public Task<List<Apprenticeship>> GetApprenticeshipsAsync()
        {
            return _context.Apprenticeships
                .Include(a => a.ApprenticeProfile)
                    .ThenInclude(ap => ap.ApplicationUser)
                .Include(a => a.MasterProfile)
                    .ThenInclude(mp => mp.ApplicationUser)
                .Include(a => a.CraftOffering)
                    .ThenInclude(o => o.Craft)
                .ToListAsync();
        }

        public async Task UpdateApprenticeshipStatusAsync(int id, string status)
        {
            var app = await _context.Apprenticeships.FindAsync(id);
            if (app != null)
            {
                app.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePaymentStatusAsync(int id, string status)
        {
            var payment = await _context.Payments
                .Include(p => p.Apprenticeship)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null) return;

            payment.Status = status;
            if (status == PaymentStatus.Success.ToString() && payment.Apprenticeship != null)
            {
                payment.Apprenticeship.Status = ApprenticeshipStatus.Active.ToString();
            }
            await _context.SaveChangesAsync();
        }

        public Task<List<Payment>> GetPaymentsAsync()
        {
            return _context.Payments
                .Include(p => p.Apprenticeship)
                    .ThenInclude(a => a.ApprenticeProfile)
                        .ThenInclude(ap => ap.ApplicationUser)
                .Include(p => p.Apprenticeship)
                    .ThenInclude(a => a.MasterProfile)
                        .ThenInclude(mp => mp.ApplicationUser)
                .ToListAsync();
        }
    }
}
