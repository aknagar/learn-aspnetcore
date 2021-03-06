﻿using System;
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
using Swashbuckle.AspNetCore.Swagger;

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
            services.RegisterAuthoriazationServiceIfEnabled(Configuration);
            services.RegisterCorsIfEnabled(Configuration);
            services.Configure<ConfigSettings>(Configuration);
            
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Route API", Version = "v1" });
            });

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

            app.UseCors("AllowSpecificOrigins");

            // It should be before UseMvc()
            app.UseErrorHandling();
            app.UseHttps();
            app.UseCorrelation();
            app.UseRequestResponseLogging();
            app.UseAuthentication();
            app.UseMvc();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Route Api V1");
            });

        }
    }
}
