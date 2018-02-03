using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Startup.Utility.Security
{
    public static class JwtBearerAuthenticationExtension
    {
        public static AuthenticationBuilder RegisterJwtBearerAuthentication(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            return builder.AddJwtBearer(options =>
            {
                options.Authority = "https://login.windows-ppe.net/" + "bedrockppedirectory.ccsctp.net";
                options.Audience = "http://OrderTransactionServicePPE.com";
                options.RequireHttpsMetadata = false;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = OnAuthenticationFailedHandler
                };
            });
        }

        public static AuthenticationBuilder RegisterJwtBearerAuthenticationIfEnabled(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("Authentication:JwtBearer:IsEnabled"))
            {
                return builder.RegisterJwtBearerAuthentication(configuration);
            }

            return builder;
        }

        private static Task OnAuthenticationFailedHandler(AuthenticationFailedContext context)
        {
            return Task.FromResult(0);
        }
    }
}
