using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace smelite_app.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (feature != null)
            {
                var user = HttpContext.User.Identity?.IsAuthenticated == true
                    ? HttpContext.User.Identity?.Name
                    : "Anonymous";
                _logger.LogError(feature.Error,
                    "Unhandled exception for user {User} at {Path}",
                    user,
                    feature.Path);
            }
            return View();
        }

        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied()
        {
            var user = HttpContext.User.Identity?.IsAuthenticated == true
                ? HttpContext.User.Identity?.Name
                : "Anonymous";
            var attempted = HttpContext.Request.Headers["Referer"].ToString();
            _logger.LogWarning("Unauthorized access by {User} to {Path} on {Time}",
                user,
                attempted,
                DateTime.UtcNow);
            return View();
        }
    }
}
