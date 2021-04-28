using ChiaPool.Clients;
using ChiaPool.Configuration;
using ChiaPool.Models;
using ChiaPool.Utils;
using Common.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    [RunPriority(10)]
    public class ConnectionService : Service, IConnectionManager
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
        [Inject]
        private readonly PlotService PlotService;

        protected override ValueTask InitializeAsync()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(new Uri(ServerOptions.PoolHost, "MHub"), x =>
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
        private Task OnReconnecting(Exception arg)
        {
            Logger.LogInformation("Reconnecting...");
            return Task.CompletedTask;
        }
        private Task OnConnectionClosed(Exception arg)
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
            var plotInfos = await PlotService.GetPlotInfosAsync();

            if (status.PlotCount != plotInfos.Length)
            {
                Logger.LogInformation("Plot count not matching with plot infos.\n" +
                                      "Updating Status...");
                await StatusService.RefreshStatusAsync();
                status = StatusService.GetCurrentStatus();
            }

            var result = await Connection.InvokeAsync<MinerUpdateResult>(MinerHubMethods.Update, status, plotInfos);

            if (!result.Successful)
            {
                Logger.LogError($"Failed to execute {MinerHubMethods.Update} via the websocket!\n" +
                                $"Reason: \"{result.Reason}\"");
                Logger.LogInformation("Restarting socket...");
                await RestartConnectionAsync();
                return;
            }

            HandleConflicts(result.Conflicts);
        }
        public async Task SendActivateRequestAsync()
        {
            var status = StatusService.GetCurrentStatus();
            var plotInfos = await PlotService.GetPlotInfosAsync();

            if (status.PlotCount != plotInfos.Length)
            {
                Logger.LogInformation("Plot count not matching with plot infos.\n" +
                                      "Updating Status...");
                await StatusService.RefreshStatusAsync();
                status = StatusService.GetCurrentStatus();
            }

            var result = await Connection.InvokeAsync<MinerActivationResult>(MinerHubMethods.Activate, status, plotInfos);

            if (!result.Successful)
            {
                Logger.LogError($"Failed to execute {MinerHubMethods.Activate} via the websocket!\n" +
                                $"Reason: \"{result.Reason}\"");
                Logger.LogInformation("Restarting socket...");
                await RestartConnectionAsync();
                return;
            }

            HandleConflicts(result.Conflicts);
            UserId = result.UserId;
        }

        private void HandleConflicts(PlotInfo[] conflicts)
        {
            if (!conflicts.Any())
            {
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("The pool reported plot conflicts:");
            foreach (var conflict in conflicts)
            {
                sb.AppendLine($"- {conflict.PublicKey}");
            }
            sb.Append("Make sure you're not using the same plots on a different miner!");

            Logger.LogError(sb.ToString());
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
