using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Startup.Utility.Security
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection RegisterAuthoriazationService(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthorizationPolicyEvaluator()
                .AddAuthorization();
            //services.AddAuthorization(options =>
            //{
            //    options.DefaultPolicy
            //    //options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            //});

            return services;
        }

        public static IServiceCollection RegisterAuthoriazationServiceIfEnabled(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("Authorization:IsEnabled"))
            {
                return services.RegisterAuthoriazationService(configuration);
            }

            return services;
        }
    }
}
