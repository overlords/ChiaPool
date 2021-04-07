using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaPool
{
    public class Program
    {
        public const int ApplicationPort = 8555;

        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            var logger = webHost.Services.GetRequiredService<ILogger<Startup>>();
            var assembly = Assembly.GetExecutingAssembly();

            var validationResult = await webHost.Services.ValidateOptionsAsync(assembly);

            if (!validationResult.IsSuccessful)
            {
                logger.LogError($"Config Validation failed: {validationResult.Reason}");
                return;
            }

            await webHost.Services.InitializeApplicationServicesAsync(assembly);
            webHost.Services.RunApplicationServices(assembly);

            await webHost.StartAsync();
            await webHost.WaitForShutdownAsync();

            webHost.Dispose();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.Listen(IPAddress.Any, ApplicationPort);
                    });
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.json", false);
                });
    }
}
