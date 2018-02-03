using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Startup.Utility.Security
{
    public static class CorsExtensions
    {
        public static IServiceCollection RegisterCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetValue<string>("Cors:AllowedOrigins");
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder => builder.WithOrigins(allowedOrigins.Split(',')));
            });

            return services;
        }

        public static IServiceCollection RegisterCorsIfEnabled(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("Cors:IsEnabled"))
            {
                return services.RegisterCors(configuration);
            }

            return services;
        }
    }
}
