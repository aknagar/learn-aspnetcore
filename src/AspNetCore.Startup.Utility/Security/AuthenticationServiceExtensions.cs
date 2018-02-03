using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Startup.Utility.Security
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection RegisterAuthenticationService(this IServiceCollection services, IConfiguration configuratioin)
        {
           services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .RegisterJwtBearerAuthenticationIfEnabled(configuratioin);

            return services;
        }
    }
}
