using Microsoft.EntityFrameworkCore;
using smelite_app.Data;
using smelite_app.Models;

namespace smelite_app.Repositories
{
    public class EmailSubscriptionRepository : IEmailSubscriptionRepository
    {
        private readonly ApplicationDbContext _context;
        public EmailSubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<EmailSubscription> GetAll()
        {
            return _context.EmailSubscriptions;
        }

        public Task<EmailSubscription?> GetByIdAsync(int id)
        {
            return _context.EmailSubscriptions.FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<EmailSubscription?> GetByEmailAsync(string email)
        {
            return _context.EmailSubscriptions.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task AddAsync(EmailSubscription subscription)
        {
            _context.EmailSubscriptions.Add(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmailSubscription subscription)
        {
            _context.EmailSubscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
