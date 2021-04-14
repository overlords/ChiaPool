using ChiaPool.Api;
using ChiaPool.Configuration;
using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaPool
{
    public class Program
    {
        public const int ApplicationPort = 8555;

        private static IHost Application;

        public static async Task Main(string[] args)
        {
            Application = CreateHostBuilder(args).Build();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();
            var assembly = Assembly.GetExecutingAssembly();

            var validationResult = await Application.Services.ValidateOptionsAsync(assembly);

            if (!validationResult.IsSuccessful)
            {
                logger.LogError($"Config Validation failed: {validationResult.Reason}");
                Application.Dispose();
                Environment.Exit(1);
            }

            InitializeServices();

            await Application.Services.InitializeApplicationServicesAsync(assembly);
            Application.Services.RunApplicationServices(assembly);

            await Application.StartAsync();
            await Application.WaitForShutdownAsync();

            Application.Dispose();
        }

        private static void InitializeServices()
        {
            var serverOptions = Application.Services.GetRequiredService<ServerOption>();
            var serverAccessor = Application.Services.GetRequiredService<ServerApiAccessor>();

            serverAccessor.SetApiUrl(serverOptions.PoolHost);
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
