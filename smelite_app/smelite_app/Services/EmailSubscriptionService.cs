using Microsoft.EntityFrameworkCore;
using smelite_app.Models;
using smelite_app.Repositories;
using smelite_app.Helpers;

namespace smelite_app.Services
{
    public class EmailSubscriptionService : IEmailSubscriptionService
    {
        private readonly IEmailSubscriptionRepository _repo;
        private readonly IEmailSender _emailSender;
        public EmailSubscriptionService(IEmailSubscriptionRepository repo, IEmailSender emailSender)
        {
            _repo = repo;
            _emailSender = emailSender;
        }

        public Task<List<EmailSubscription>> GetAllAsync()
        {
            return _repo.GetAll().ToListAsync();
        }

        public async Task SubscribeAsync(string email)
        {
            email = email.Trim().ToLowerInvariant();
            var existing = await _repo.GetByEmailAsync(email);
            if (existing != null)
            {
                if (!existing.IsActive)
                {
                    existing.IsActive = true;
                    await _repo.UpdateAsync(existing);
                    await _emailSender.SendEmailAsync(email, EmailMessages.SubscriptionConfirmSubject, EmailMessages.SubscriptionConfirmBody);
                }
                return;
            }

            var subscription = new EmailSubscription { Email = email };
            await _repo.AddAsync(subscription);
            await _emailSender.SendEmailAsync(email, EmailMessages.SubscriptionConfirmSubject, EmailMessages.SubscriptionConfirmBody);
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
