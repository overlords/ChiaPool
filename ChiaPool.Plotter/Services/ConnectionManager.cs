using ChiaPool.Clients;
using ChiaPool.Configuration;
using ChiaPool.Utils;
using Common.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class ConnectionManager : Service, IConnectionManager
    {
        [Inject]
        private readonly ServerOption ServerOptions;
        [Inject]
        private readonly StatusService StatusService;
        [Inject]
        private readonly PlotService PlotService;
        [Inject]
        private readonly AuthOption AuthOptions;


        private HubConnection Connection;

        protected override ValueTask InitializeAsync()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"https://{ServerOptions.PoolHost}:{ServerOptions.ManagerPort}/PHub", x =>
                {
                    x.Headers.Add("Authorization", $"Plotter {AuthOptions.Token}");
                    x.Transports = HttpTransportType.WebSockets;
                })
                .WithAutomaticReconnect(new PersistentRetryPolicy())
                .AddJsonProtocol()
                .ConfigureLogging(x =>
                {
                    x.AddProvider(new ExistingLoggerProvider(Logger));
                })
                .Build();

            Connection.On(PlotterMethods.RequestPlot, () => HandlePlotRequestAsync());
            Connection.On<long>(PlotterMethods.DeletePlot, plotId => HandleDeleteRequestAsync(plotId));


            Connection.Closed += OnConnectionClosed;
            Connection.Reconnecting += OnReconnecting;
            Connection.Reconnected += OnReconnected;
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

        private async Task HandlePlotRequestAsync()
        {
            var remotePlot = await PlotService.ActivateRemotePlotAsync();
            await SendStatusUpdateAsync();
            await Connection.SendAsync(PlotterHubMethods.OfferPlot, remotePlot);
        }
        private async Task HandleDeleteRequestAsync(long plotId)
        {
            await PlotService.DeletePlotAsync(plotId);
        }

        public async Task SendStatusUpdateAsync()
        {
            var status = StatusService.GetCurrentStatus();
            await Connection.SendAsync(PlotterHubMethods.Update, status);
        }
        public async Task SendActivateRequestAsync()
        {
            var status = StatusService.GetCurrentStatus();
            await Connection.SendAsync(PlotterHubMethods.Activate, status);
        }
    }
}
