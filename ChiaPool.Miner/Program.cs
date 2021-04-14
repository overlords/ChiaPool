using Chia.NET.Clients;
using ChiaPool.Api;
using ChiaPool.Configuration;
using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaPool
{
    public class Program
    {
        public const int ApplicationPort = 8888;
        private static IHost Application;

        public static async Task Main(string[] args)
        {
            Application = CreateHostBuilder(args).Build();

            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();
            var assembly = Assembly.GetExecutingAssembly();
            var chiaNetAssembly = Assembly.GetAssembly(typeof(HarvesterClient));
            var chiaPoolNetAssembly = Assembly.GetAssembly(typeof(ServerApiAccessor));

            var validationResult = await Application.Services.ValidateOptionsAsync(assembly);

            if (!validationResult.IsSuccessful)
            {
                logger.LogError($"Config Validation failed: {validationResult.Reason}");
                Application.Dispose();
                Environment.Exit(1);
            }

            InitializeServices();

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

            await Application.Services.InitializeApplicationServicesAsync(chiaNetAssembly);

            if (!await WaitForHarvesterAsync())
            {
                Application.Dispose();
                Environment.Exit(1);
            }

            await Application.Services.InitializeApplicationServicesAsync(assembly);
            await Application.Services.InitializeApplicationServicesAsync(chiaPoolNetAssembly);

            Application.Services.RunApplicationServices(assembly);
            Application.Services.RunApplicationServices(chiaPoolNetAssembly);
            Application.Services.RunApplicationServices(chiaNetAssembly);

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
                .ConfigureLogging(builder =>
                {
                    builder.AddFile(options =>
                    {
                        options.Append = true;
                        options.FormatterName = "simple";
                        options.IncludeScopes = true;
                        options.MaxFileSizeInMB = 5;
                        options.MaxNumberFiles = 3;
                        options.Path = "../.chia/mainnet/log/pool.log";
                    });
                })
                .ConfigureAppConfiguration(config =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.json", false);
                });

        private static async Task<bool> RunInitAsync()
        {
            var client = Application.Services.GetRequiredService<ServerApiAccessor>();
            var authOptions = Application.Services.GetRequiredService<AuthOption>();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();

            try
            {
                logger.LogInformation("Downloading ca certificate...");
                using var zipArchive = await client.GetCACertificateArchiveAsync(authOptions.Token);
                logger.LogInformation("Extracting ca certificate...");
                zipArchive.ExtractToDirectory("/root/chia-blockchain/ca/", true);
                logger.LogInformation("Finished updating ca!");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating ca!");
                return false;
            }
        }
        private static async Task<bool> WaitForHarvesterAsync()
        {
            var client = Application.Services.GetRequiredService<HarvesterClient>();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();

            logger.LogInformation("Waiting for harvester to spin up");

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(5000);
                try
                {
                    await client.GetConnections();
                    logger.LogInformation("Done");
                    return true;
                }
                catch (Exception ex)
                {
                    if (i == 9)
                    {
                        logger.LogError(ex, $"Failed connecting to chia!");
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
