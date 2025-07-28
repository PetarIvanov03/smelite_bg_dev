using Microsoft.EntityFrameworkCore;
using smelite_app.Models;
using System.Threading.Tasks;

namespace smelite_app.Repositories
{
    public interface IEmailSubscriptionRepository
    {
        IQueryable<EmailSubscription> GetAll();
        Task<EmailSubscription?> GetByIdAsync(int id);
        Task<EmailSubscription?> GetByEmailAsync(string email);
        Task AddAsync(EmailSubscription subscription);
        Task UpdateAsync(EmailSubscription subscription);
    }
}
