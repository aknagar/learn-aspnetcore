using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Startup.Utility.Security
{
    public static class JwtBearerAuthenticationExtension
    {
        public static AuthenticationBuilder RegisterJwtBearerAuthentication(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            return builder.AddJwtBearer(options =>
            {
                options.Authority = configuration.GetValue<string>("IdentityUrl");
                options.Audience = "locations";
                options.RequireHttpsMetadata = false;
            });
        }

        public static AuthenticationBuilder RegisterJwtBearerAuthenticationIfEnabled(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("IsJwtBearerAuthEnabled"))
            {
                return builder.RegisterJwtBearerAuthentication(configuration);
            }

            return builder;
        }
    }
}
