using smelite_app.Models;

namespace smelite_app.Services
{
    public interface IEmailSubscriptionService
    {
        Task<List<EmailSubscription>> GetAllAsync();
        Task SubscribeAsync(string email);
        Task ToggleActiveAsync(int id, bool isActive);
        Task<List<string>> GetActiveEmailsAsync();
    }
}
