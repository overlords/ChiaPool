using ChiaPool.Api;
using ChiaPool.Configuration;
using ChiaPool.Services;
using CliFx;
using Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ChiaPool
{
    public class Program
    {
        private static CliApplication Application;
        private static IServiceProvider Provider;

        public static async Task Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Provider = MakeServiceProvider();

            var validationResult = await Provider.ValidateOptionsAsync(assembly);

            if (!validationResult.IsSuccessful)
            {
                Console.WriteLine($"Config Validation failed: {validationResult.Reason}");
                return;
            }

            Application = new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(Provider.GetService)
                .SetVersion("v0.8.0")
                .SetExecutableName("ChiaPool")
                .SetTitle("ChiaPool CLI")
                .Build();

            await Application.RunAsync();
        }

        private static void InitializeServices()
        {
            var serverOptions = Provider.GetRequiredService<ServerOption>();
            var minerAccessor = Provider.GetRequiredService<MinerApiAccessor>();
            var serverAccessor = Provider.GetRequiredService<ServerApiAccessor>();

            minerAccessor.SetApiUrl(serverOptions.MinerHost);
            serverAccessor.SetApiUrl(serverOptions.PoolHost);
        }

        private static IServiceProvider MakeServiceProvider()
        {
            var chiaPoolNetAssembly = Assembly.GetAssembly(typeof(MinerApiAccessor));
            var assembly = Assembly.GetExecutingAssembly();

            var configuration = new ConfigurationBuilder().Build();
            var services = new ServiceCollection();

            services.AddApplicationServices(chiaPoolNetAssembly);
            services.AddApplication(configuration, assembly);
            services.AddSingleton<HttpClient>();
            services.AddSingleton<ConfigurationService>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetInterface(nameof(ICommand)) != null && !x.IsAbstract))
            {
                services.AddTransient(type);
            }

            return services.BuildServiceProvider();
        }
    }
}
