using Microsoft.AspNetCore.Mvc;
using smelite_app.Services;

namespace smelite_app.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];
            await _paymentService.HandleWebhookAsync(json, signature);
            return Ok();
        }
    }
}
