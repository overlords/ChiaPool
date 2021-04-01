using Chia.NET.Clients;
using ChiaMiningManager.Models;
using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaMiningManager
{
    public class Program
    {
        public const int ApplicationPort = 8666;
        private static IHost Application;

        public static async Task Main(string[] args)
        {
            Application = CreateHostBuilder(args).Build();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();
            var assembly = Assembly.GetExecutingAssembly();
            var chiaNetAssembly = Assembly.GetAssembly(typeof(WalletClient));

            var validationResult = await Application.Services.ValidateOptionsAsync(assembly);

            if (!validationResult.IsSuccessful)
            {
                logger.LogError($"Config Validation failed: {validationResult.Reason}");
            }

            await MigrateDatabaseAsync();
            await Application.Services.InitializeApplicationServicesAsync(assembly);
            await Application.Services.InitializeApplicationServicesAsync(chiaNetAssembly);

            await RunInitAsync();

            Application.Services.RunApplicationServices(assembly);
            Application.Services.RunApplicationServices(chiaNetAssembly);

            await Application.StartAsync();
            await Application.WaitForShutdownAsync();

            Application.Dispose();
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
                .ConfigureAppConfiguration(config =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.json", false);
                });

        private static async Task MigrateDatabaseAsync()
        {
            using var scope = Application.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();

            await dbContext.Database.MigrateAsync();
        }

        private static async Task RunInitAsync()
        {
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();
            var farmerApiClient = Application.Services.GetRequiredService<FarmerClient>();

            logger.LogInformation("Configuring farmer reward target");

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(5000);
                try
                {
                    await farmerApiClient.SetRewardTargets(Environment.GetEnvironmentVariable("wallet_address"));
                    logger.LogInformation("Done");
                    return;
                }
                catch
                {
                    logger.LogWarning($"Connection failed. Trying again in 5 seconds. {9 - i} retries left");
                }
            }

            throw new Exception("Conenction Error!");
        }
    }
}
