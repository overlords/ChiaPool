using ChiaPool.Clients;
using ChiaPool.Configuration;
using ChiaPool.Configuration.Options;
using Common.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class ConnectionManager : Service
    {
        private HubConnection Connection;

        [Inject]
        private readonly ServerOption ServerOptions;
        [Inject]
        private readonly AuthOption AuthOptions;
        [Inject]
        private readonly StatusService StatusService;

        protected override async ValueTask InitializeAsync()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"https://{ServerOptions.PoolHost}:{ServerOptions.ManagerPort}/MHub", x =>
                {
                    x.Headers.Add("Authorization", $"Miner {AuthOptions.Token}");
                })
                .WithAutomaticReconnect(new PersistentRetryPolicy())
                .Build();

            await Connection.StartAsync();

            Connection.Closed += OnConnectionClosed;
            Connection.Reconnecting += OnReconnecting;
            Connection.Reconnected += OnReconnected;
        }

        private async Task OnReconnected(string arg)
        {
            Logger.LogInformation("Successfully reconnected");
            await SendActivateRequestAsync();
        }

        private Task OnReconnecting(System.Exception arg)
        {
            Logger.LogInformation("Reconnecting...");
            return Task.CompletedTask;
        }

        private Task OnConnectionClosed(System.Exception arg)
        {
            Logger.LogWarning("Connection to ChiaPool Manager closed!");
            return Task.CompletedTask;
        }

        public async Task SendStatusUpdateAsync()
        {
            var status = await StatusService.GetStatusAsync();
            await Connection.SendAsync(PlotterHubMethods.Update, status);
        }
        private async Task SendActivateRequestAsync()
        {
            var status = await StatusService.GetStatusAsync();
            await Connection.SendAsync(PlotterHubMethods.Activate, status);
        }

        protected override async ValueTask RunAsync()
        {
            await SendActivateRequestAsync();
        }
    }
}
