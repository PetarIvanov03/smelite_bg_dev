using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using smelite_app.Data;
using smelite_app.Enums;
using smelite_app.Models;
using Stripe;
using Stripe.Checkout;

namespace smelite_app.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> CreateCheckoutSessionAsync(int paymentId, string successUrl, string cancelUrl)
        {
            var payment = await _context.Payments
                .Include(p => p.RecipientProfile)
                .FirstOrDefaultAsync(p => p.Id == paymentId);
            if (payment == null) throw new InvalidOperationException("Payment not found");

            var acctId = payment.RecipientProfile?.StripeAccountId;
            if (string.IsNullOrWhiteSpace(acctId) || !acctId.StartsWith("acct_"))
                throw new InvalidOperationException("Master does not have a valid Stripe account. Cannot process payment.");

            try
            {
                var accountService = new Stripe.AccountService();
                var account = accountService.Get(acctId);
                if (account == null || account.Deleted == true)
                    throw new InvalidOperationException("Master does not have a valid Stripe account. Cannot process payment.");
            }
            catch (StripeException ex)
            {
                Console.WriteLine("Stripe validation error: " + ex.Message);
                throw new InvalidOperationException("Master does not have a valid Stripe account. Cannot process payment.");
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(payment.AmountTotal * 100),
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Apprenticeship"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrl + "?sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = cancelUrl,
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = (long)(payment.PlatformFee * 100),
                    TransferData = new SessionPaymentIntentDataTransferDataOptions
                    {
                        Destination = payment.RecipientProfile!.StripeAccountId
                    }
                }
            };

            var service = new SessionService();

            try
            {
                var session = await service.CreateAsync(options);
                payment.TransactionId = session.Id;
                await _context.SaveChangesAsync();
                return session.Url;
            }
            catch (Exception ex)
            {
                Console.WriteLine("STRIPE EXCEPTION: " + ex.ToString());
                throw; // Може временно да махнеш throw, за да не хвърляш към ErrorController, а да видиш грешката
            }

        }

        public async Task HandleWebhookAsync(string json, string signature)
        {
            var secret = _configuration["Stripe:WebhookSecret"];
            var stripeEvent = EventUtility.ConstructEvent(json, signature, secret);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session != null)
                {
                    var payment = await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == session.Id);
                    if (payment != null)
                    {
                        payment.Status = PaymentStatus.Success.ToString();
                        payment.Method = "Card";
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
