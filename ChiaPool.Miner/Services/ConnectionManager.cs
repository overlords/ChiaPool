using ChiaPool.Clients;
using ChiaPool.Configuration;
using ChiaPool.Configuration.Options;
using ChiaPool.Models;
using ChiaPool.Utils;
using Common.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    [RunPriority(10)]
    public class ConnectionManager : Service, IConnectionManager
    {
        private const int ConnectionRestartDelay = 5000;

        private HubConnection Connection;
        private long UserId;

        [Inject]
        private readonly ServerOption ServerOptions;
        [Inject]
        private readonly AuthOption AuthOptions;
        [Inject]
        private readonly StatusService StatusService;

        protected override ValueTask InitializeAsync()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"https://{ServerOptions.PoolHost}:{ServerOptions.ManagerPort}/MHub", x =>
                {
                    x.Headers.Add("Authorization", $"Miner {AuthOptions.Token}");
                    x.Transports = HttpTransportType.WebSockets;
                })
                .WithAutomaticReconnect(new PersistentRetryPolicy())
                .AddJsonProtocol()
                .ConfigureLogging(x =>
                {
                    x.AddProvider(new ExistingLoggerProvider(Logger));
                })
                .Build();

            Connection.Closed += OnConnectionClosed;
            Connection.Reconnecting += OnReconnecting;
            Connection.Reconnected += OnReconnected;

            Connection.On<DisconnectRequest>(MinerMethods.Disconnect, HandleDisconnectRequestAsync);

            return ValueTask.CompletedTask;
        }
        protected override async ValueTask RunAsync()
        {
            await Connection.StartAsync();
            await SendActivateRequestAsync();
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

        private async Task HandleDisconnectRequestAsync(DisconnectRequest request)
        {
            Logger.LogWarning($"The server requested a disconnect!\n" +
                              $"Reason: {request.Reason}");
            await RestartConnectionAsync();
        }

        public async Task SendStatusUpdateAsync()
        {
            var status = StatusService.GetCurrentStatus();
            await Connection.SendAsync(MinerHubMethods.Update, status);
        }
        public async Task SendActivateRequestAsync()
        {
            var status = StatusService.GetCurrentStatus();
            var result = await Connection.InvokeAsync<ActivationResult>(MinerHubMethods.Activate, status);

            if (!result.Successful)
            {
                Logger.LogError($"Failed to execute {MinerHubMethods.Activate} via the websocket!\n" +
                                $"Reason: \"{result.Reason}\"");
                await RestartConnectionAsync();
                return;
            }

            UserId = result.UserId;
        }

        private async Task RestartConnectionAsync()
        {
            await Connection.StopAsync();
            await Task.Delay(ConnectionRestartDelay);
            await Connection.StartAsync();
            await SendActivateRequestAsync();
        }

        public long GetCurrentUserId()
            => UserId;
    }
}
