using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HubalooAPI.Middleware
{
    public class FirstCustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public FirstCustomMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("FirstCustomMiddleware");
            _logger.LogInformation("Custom middleware started =>");
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("Custom middleware executed...");

            await _next(httpContext);
        }
    }
}