using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace smelite_app.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;

        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User.Identity?.IsAuthenticated == true
                ? context.HttpContext.User.Identity?.Name
                : "Anonymous";
            var action = context.ActionDescriptor.DisplayName;
            var parameters = context.ActionArguments;
            var paramsJson = JsonSerializer.Serialize(parameters);
            _logger.LogInformation(
                "User: {User}, Action: {Action}, Params: {Params}, Time: {Time}",
                user,
                action,
                paramsJson,
                DateTime.UtcNow);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
