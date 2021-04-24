using Chia.NET.Clients;
using ChiaPool.Models;
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

namespace ChiaPool
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
                Application.Dispose();
                Environment.Exit(1);
            }

            await MigrateDatabaseAsync();
            await Application.Services.InitializeApplicationServicesAsync(chiaNetAssembly);

            if (args.Length == 1 && args[0] == "init")
            {
                bool success = await RunInitAsync();
                Application.Dispose();

                if (!success)
                {
                    Environment.Exit(1);
                }
                return;
            }

            if (!await WaitForChiaClientAsync<FullNodeClient>("fullnode"))
            {
                Application.Dispose();
                Environment.Exit(1);
            }

            await Application.Services.InitializeApplicationServicesAsync(assembly);

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

        private static Task<bool> RunInitAsync() 
            => WaitForChiaClientAsync<WalletClient>("wallet");

        private static async Task<bool> WaitForChiaClientAsync<T>(string chiaNodeName) where T : ChiaApiClient
        {
            var client = Application.Services.GetRequiredService<T>();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();

            logger.LogInformation($"Waiting for {chiaNodeName} to spin up");

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(5000);
                try
                {
                    await client.GetConnections();
                    logger.LogInformation("Connection established!");
                    return true;
                }
                catch (Exception ex)
                {
                    if (i == 9)
                    {
                        logger.LogError(ex, $"Failed connecting to {chiaNodeName}!");
                    }
                    else
                    {
                        logger.LogWarning($"Connection failed. Trying again in 5 seconds. {9 - i} retries left");
                    }
                }
            }

            return false;
        }
    }
}
