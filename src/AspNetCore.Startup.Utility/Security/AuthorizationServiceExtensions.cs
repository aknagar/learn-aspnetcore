using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Startup.Utility.Security
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection RegisterAuthoriazationService(this IServiceCollection services, IConfiguration configuratioin)
        {
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            });

            return services;
        }

        public static IServiceCollection RegisterAuthoriazationServiceIfEnabled(this IServiceCollection services, IConfiguration configuratioin)
        {
            services.AddAuthorization(options =>
            {
                //options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            });

            return services;
        }
    }
}
