using Microsoft.AspNetCore.Http;
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
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is NotImplementedException) code = HttpStatusCode.NotImplemented;
            else if (exception is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;

            var result = exception.Message;
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

    }
}
