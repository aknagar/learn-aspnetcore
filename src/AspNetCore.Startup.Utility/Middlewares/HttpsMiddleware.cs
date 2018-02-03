using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Startup.Utility.Middlewares
{
    /// <summary>
    /// This is a short-circuit Middleware.
    /// If the request is NOT on HTTPS protocol, request would NOT be forwarded to next middleware in the pipeline
    /// </summary>
    public class HttpsMiddleware
    {
        private readonly RequestDelegate next;
        private const string Https = "https";
        private const string XForwardedProto = "X-Forwarded-Proto";

        public HttpsMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            if (context.Request.IsHttps || IsForwardedSsl(context))
            {
                await next(context);
                               
            }
            else
            {
                await HandleAsync(context);
            }            
        }

        private static Task HandleAsync(HttpContext context)
        {            
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return context.Response.WriteAsync("HTTPS Required");
        }

        /// <summary>
        /// This is helpful when Load Balancer does SSL Offloading.
        /// SSL generally inject a “X-Forwarded-Proto” header into the request with the value “http” or “https” to indicate the protocol of the original request. 
        /// </summary>
        private static bool IsForwardedSsl(HttpContext context)
        {            
            var forwardedSsl = false;
            var values = context.Request.Headers[XForwardedProto];
            var enumarator = values.ToArray().GetEnumerator();
            while(enumarator.MoveNext())
            {
                forwardedSsl = enumarator.Current.ToString().Equals(Https, StringComparison.InvariantCultureIgnoreCase);
            }

            return forwardedSsl;
        }
    }
}
