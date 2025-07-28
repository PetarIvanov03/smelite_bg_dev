using Microsoft.EntityFrameworkCore;
using smelite_app.Models;
using smelite_app.Repositories;

namespace smelite_app.Services
{
    public class EmailSubscriptionService : IEmailSubscriptionService
    {
        private readonly IEmailSubscriptionRepository _repo;
        public EmailSubscriptionService(IEmailSubscriptionRepository repo)
        {
            _repo = repo;
        }

        public Task<List<EmailSubscription>> GetAllAsync()
        {
            return _repo.GetAll().ToListAsync();
        }

        public async Task SubscribeAsync(string email)
        {
            var existing = await _repo.GetByEmailAsync(email);
            if (existing != null)
            {
                if (!existing.IsActive)
                {
                    existing.IsActive = true;
                    await _repo.UpdateAsync(existing);
                }
                return;
            }

            var subscription = new EmailSubscription { Email = email };
            await _repo.AddAsync(subscription);
        }

        public async Task ToggleActiveAsync(int id, bool isActive)
        {
            var sub = await _repo.GetByIdAsync(id);
            if (sub != null)
            {
                sub.IsActive = isActive;
                await _repo.UpdateAsync(sub);
            }
        }

        public Task<List<string>> GetActiveEmailsAsync()
        {
            return _repo.GetAll().Where(s => s.IsActive).Select(s => s.Email).ToListAsync();
        }
    }
}
