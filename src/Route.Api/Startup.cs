using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreUtilities;
using AspNetCoreUtilities.Filters;
using AspNetCoreUtilities.Middlewares;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Route.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // You can check the registered configuration provider
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.

        // When using a third-party DI container, you must change ConfigureServices so that it returns IServiceProvider instead of void.
        // This lets ASP.NET know to use your container instead of the built in one.
        public IServiceProvider ConfigureServices(IServiceCollection services)
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
                            mvcOptions.Filters.Add(new RequireHttpsAttribute());
                            //mvcOptions.Filters.Add(typeof(ExceptionFilter));
                        });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            });

            services.Configure<ConfigSettings>(Configuration);

            // prevent from mapping "sub" claim to nameidentifier.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("IdentityUrl");
                    options.Audience = "locations";
                    options.RequireHttpsMetadata = false;
                });

            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<RouteApiModule>();
            containerBuilder.Populate(services);
            this.ApplicationContainer = containerBuilder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            // It should be before UseMvc()
            //app.UseErrorHandling();

            app.UseMvc();
        }
    }
}
