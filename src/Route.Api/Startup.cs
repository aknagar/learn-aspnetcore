using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Startup.Utility;
using AspNetCore.Startup.Utility.Security;
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
            services.RegisterMvcService(Configuration); 
            services.RegisterAuthenticationService(Configuration);
            services.RegisterAuthoriazationService(Configuration);
            services.RegisterCorsIfEnabled(Configuration);
            services.Configure<ConfigSettings>(Configuration);
            
            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<RouteApiModule>();
            containerBuilder.RegisterModule<HttpPipelineUtilityModule>();
            containerBuilder.Populate(services);
            this.ApplicationContainer = containerBuilder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            // here sequence of middlewares is important
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // It should be before UseMvc()
            //app.UseErrorHandling();

            app.UseHttps();

            app.UseCorrelation();

            app.UseAuthentication();
            app.UseCors("AllowSpecificOrigin");

            app.UseMvc();
        }
    }
}
