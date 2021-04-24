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
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaPool
{
    public class Program
    {
        public const string ConfigFilePath = "/root/.chia/mainnet/config/config.yaml";
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

            InitializeServerApiAccessor();
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
            
            if (!await WaitForChiaClientAsync<HarvesterClient>("harvester"))
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

        private static void InitializeServerApiAccessor()
        {
            var serverOptions = Application.Services.GetRequiredService<ServerOption>();
            var serverAccessor = Application.Services.GetRequiredService<ServerApiAccessor>();

            serverAccessor.SetApiUrl(serverOptions.PoolHost);
            serverAccessor.SetAuthenticationScheme("Miner");
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
                        options.Path = "/root/.chia/mainnet/log/pool.log";
                    });
                })
                .ConfigureAppConfiguration(config =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.json", false);
                });

        private static async Task<bool> RunInitAsync()
            => await WaitForChiaClientAsync<FarmerClient>("farmer") &&
               await TrySetFarmerTargetAsync() &&
               await TrySetFarmerFullNodePeer() &&
               await TrySetPlottingKeyEnvironmentVariable();

        private static async Task<bool> TrySetFarmerTargetAsync()
        {
            var serverAccessor = Application.Services.GetRequiredService<ServerApiAccessor>();
            var farmerApiClient = Application.Services.GetRequiredService<FarmerClient>();
            var authOptions = Application.Services.GetRequiredService<AuthOption>();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();

            logger.LogInformation("Downloading target address...");
            string walletAddress = await serverAccessor.GetPoolWalletAddressAsync(authOptions.Token);

            if (string.IsNullOrWhiteSpace(walletAddress))
            {
                logger.LogError("The server did not send a valid wallet address!");
                return false;
            }

            try
            {
                logger.LogInformation($"Setting farmer target to {walletAddress}...");
                await farmerApiClient.SetRewardTargets(walletAddress);

                logger.LogInformation("Done!");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while setting target address!");
                return false;
            }
        }
        private static async Task<bool> TrySetFarmerFullNodePeer()
        {
            var serverOptions = Application.Services.GetRequiredService<ServerOption>();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();

            logger.LogInformation($"Setting farmer fullnode peer to {serverOptions.FullNodeHost}:{serverOptions.FullNodePort}!");

            try
            {
                //I know this is terrible, feel free to make it better yourself. I am a lazy person!
                logger.LogInformation("Reading chia config file...");
                string[] configFileLines = await File.ReadAllLinesAsync(ConfigFilePath);

                logger.LogInformation("Patching chia config file...");
                for (int i = 0; i < configFileLines.Length; i++)
                {
                    string line = configFileLines[i];

                    if (!line.Contains("full_node_peer:"))
                    {
                        continue;
                    }

                    int spaceCount = configFileLines[i + 1].TakeWhile(x => x == ' ').Count();
                    string spaces = new string(' ', spaceCount);

                    configFileLines[i + 1] = $"{spaces}host: {serverOptions.FullNodeHost}";
                    configFileLines[i + 2] = $"{spaces}port: {serverOptions.FullNodePort}";
                }

                logger.LogInformation("Overriding config file...");
                await File.WriteAllLinesAsync(ConfigFilePath, configFileLines);

                logger.LogInformation("Done!");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error while setting farmer fullnode peer!");
                return false;
            }
        }

        private static async Task<bool> TrySetPlottingKeyEnvironmentVariable()
        {
            var serverAccessor = Application.Services.GetRequiredService<ServerApiAccessor>();
            var authOptions = Application.Services.GetRequiredService<AuthOption>();
            var logger = Application.Services.GetRequiredService<ILogger<Startup>>();

            logger.LogInformation("Retrieving plotting keys...");
            string plottingKeys = await serverAccessor.GetPlottingKeysAsync(authOptions.Token);

            if (string.IsNullOrWhiteSpace(plottingKeys))
            {
                logger.LogError("The server did not send valid plottings keys!");
                return false;
            }

            logger.LogInformation("Storing plotting keys to environment variable...");
            Environment.SetEnvironmentVariable("plotting_keys", plottingKeys);

            logger.LogInformation("Done!");
            return true;
        }

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
