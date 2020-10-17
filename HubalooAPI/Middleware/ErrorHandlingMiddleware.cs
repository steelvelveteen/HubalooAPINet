using System;
using System.Net;
using System.Threading.Tasks;
using HubalooAPI.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HubalooAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var httpStatusCode = HttpStatusCode.InternalServerError;
            var errorMessage = exception.Message;
            if (exception is ApiException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;

            }
            else if (exception is RepositoryException)
            {

            }
            else if (exception is UnauthorizedAccessException)
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
            }
            else if (exception is PreconditionException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
            }

            var responseBody = JsonConvert.SerializeObject(new
            {
                message = errorMessage
            });

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)httpStatusCode;

            return httpContext.Response.WriteAsync(responseBody);
        }
    }
}