using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Startup.Utility.Middlewares
{ 
    /// <summary>
    /// We are using middleware for exception handling, rather thant MVC filter because
    /// it does not handle errors thrown in filters.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        private const string ErrorMessage = "Oops, something went wrong. If problem persists contact customer support";

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is NotImplementedException) statusCode = HttpStatusCode.NotImplemented;
            else if (exception is UnauthorizedAccessException) statusCode = HttpStatusCode.Unauthorized;
            
            context.Response.StatusCode = (int)statusCode;
            _logger.LogError(exception, "Unhandled Exception");

            return context.Response.WriteAsync(ErrorMessage);
        }

    }
}
