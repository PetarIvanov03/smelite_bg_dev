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

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var user = HttpContext.User.Identity?.IsAuthenticated == true
                ? HttpContext.User.Identity?.Name
                : "Anonymous";
            var path = HttpContext.Request.Path;

            _logger.LogWarning("User {User} tried to access {Path}. Status code: {StatusCode}", user, path, statusCode);

            Response.StatusCode = statusCode;

            switch (statusCode)
            {
                case 404:
                    ViewBag.Message = "?????????? ?? ? ???????? (404)";
                    return View("NotFound");
                case 403:
                    ViewBag.Message = "???????? ? ??????? (403)";
                    return View("AccessDenied");
                default:
                    ViewBag.Message = $"???????? ??????: {statusCode}";
                    return View("Error");
            }
        }


    }
}
