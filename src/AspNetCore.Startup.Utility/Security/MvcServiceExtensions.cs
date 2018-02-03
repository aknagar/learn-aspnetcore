using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Startup.Utility.Security
{
    public static class MvcServiceExtensions
    {
        public static IServiceCollection RegisterMvcService(this IServiceCollection services, IConfiguration configuratioin)
        {
            services.AddMvcCore()
                        .AddJsonFormatters()
                        .AddMvcOptions(mvcOptions =>
                        {
                            // This protects all the Controllers by default.
                            var policy = new AuthorizationPolicyBuilder()
                                                .RequireAuthenticatedUser()
                                                .Build();
                            mvcOptions.Filters.Add(new AuthorizeFilter(policy));
                        })
                        .AddApiExplorer();  //Required for Swagger UI

            return services;
        }
    }
}
