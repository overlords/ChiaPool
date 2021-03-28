using Common.Configuration;
using Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace ChiaMiningManager
{
    public class AppStartup
    {
        private readonly string ConfigPath = "Config.Yaml";

        private IConfiguration Configuration;
        private IServiceProvider Provider;
        private ILogger Logger;

        public async Task InitializeAsync()
        {
            bool generatedConfig = await TryGenerateConfigAsync();

            Configuration = MakeConfiguration();
            Provider = MakeProvider();
            Logger = Provider.GetRequiredService<ILogger<AppStartup>>();

            if (generatedConfig)
            {
                Logger.LogWarning($"A config file has been generated to {ConfigPath}.\n" +
                                  $"Add the required values and restart the application!");
                await StopAsync(0);
            }
            if (!(await Provider.ValidateOptionsAsync(Assembly.GetExecutingAssembly())).IsSuccessful)
            {
                await StopAsync(1);
            }

            await Provider.InitializeApplicationServicesAsync(Assembly.GetExecutingAssembly());
        }

        public async Task RunAsync()
        {
            Provider.RunApplicationServices(Assembly.GetExecutingAssembly());

            await Task.Delay(-1);
        }

        public Task StopAsync(int exitCode)
        {
            var loggerFactory = Provider.GetRequiredService<ILoggerFactory>();
            loggerFactory.Dispose();
            Debug.Flush();

            Environment.Exit(exitCode);


            return Task.CompletedTask;
        }

        public IConfiguration MakeConfiguration()
            => new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddYamlFile(ConfigPath)
                .Build();

        public IServiceProvider MakeProvider()
            => new ServiceCollection()
                .AddApplicationOptions(Configuration, Assembly.GetExecutingAssembly())
                .AddApplicationServices(Assembly.GetExecutingAssembly())
                .AddSingleton<HttpClient>()
                .AddLogging(options =>
                {
                    options.ClearProviders();
                    options.AddConsole();
                })
                .BuildServiceProvider();

        public async ValueTask<bool> TryGenerateConfigAsync()
        {
            if (File.Exists(ConfigPath))
            {
                return false;
            }

            var serializer = new Serializer();
            object defaultConfiguration = Option.GenerateDefaultOptions();

            string yamlConfiguration = serializer.Serialize(defaultConfiguration);

            await File.WriteAllTextAsync(ConfigPath, yamlConfiguration);
            return true;
        }
    }
}
