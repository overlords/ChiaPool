using ChiaPool.Api;
using ChiaPool.Commands;
using ChiaPool.Services;
using CliFx;
using Common.Extensions;
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
            Provider = MakeServiceProvider();
            Application = new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(Provider.GetService)
                .Build();

            await Application.RunAsync();
        }

        private static IServiceProvider MakeServiceProvider()
        {
            var chiaPoolNetAssembly = Assembly.GetAssembly(typeof(ClientApiAccessor));
            var services = new ServiceCollection();

            services.AddSingleton<HttpClient>();
            services.AddApplicationServices(chiaPoolNetAssembly);
            services.AddSingleton<ConfigurationService>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetInterface(nameof(ICommand)) != null && !x.IsAbstract))
            {
                if (type == typeof(InstallCommand) && !SupportInstall())
                {
                    continue;
                }

                services.AddTransient(type);
            }

            return services.BuildServiceProvider();
        }

        private static bool SupportInstall()
        {
            if (!OperatingSystem.IsWindows())
            {
                return false;
            }

            return !ConfigurationService.IsInstalled();
        }
    }
}
