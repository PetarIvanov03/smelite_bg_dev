using System.Threading.Tasks;

namespace smelite_app.Services
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSessionAsync(int paymentId, string successUrl, string cancelUrl);
        Task HandleWebhookAsync(string json, string signature);
    }
}
