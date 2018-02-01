using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Startup.Utility.Middlewares
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate next;

        private const string CorrelationHeader = "x-ms-correlation-id";

        public CorrelationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            var values = context.Request.Headers[CorrelationHeader];
            var enumarator = values.ToArray().GetEnumerator();
            while (enumarator.MoveNext())
            {
                context.TraceIdentifier = enumarator.Current.ToString();
            }

            // Set correlation id in response
            context.Response.Headers.Append(CorrelationHeader, context.TraceIdentifier);

            await next(context);
        }
    }
}
