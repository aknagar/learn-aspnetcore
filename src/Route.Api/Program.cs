using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Route.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                var webhost = BuildWebHost(args);
                webhost.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())                
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    //configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    //     .AddJsonFile("appsettings.json", optional: false);
                    //     .AddEnvironmentVariables();
                    //var config = configBuilder.Build();

                    //var appVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
                    //var versionPrefix = appVersion.Replace(".", string.Empty);

                    //configBuilder.AddAzureKeyVault(
                    //    config["Vault"],
                    //    config["ClientId"],
                    //    config["ClientSecret"],
                    //    new PrefixKeyVaultSecretManager(versionPrefix));
                })
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                        .ReadFrom.KeyValuePairs(hostingContext.Configuration.AsEnumerable())
                        .Enrich.FromLogContext()
                        .WriteTo.ApplicationInsightsTraces(hostingContext.Configuration["InstrumentationKey"]))
                .UseStartup<Startup>()
                .Build();
    }
}
