using ChiaPool.Clients;
using ChiaPool.Configuration;
using ChiaPool.Models;
using ChiaPool.Plotter.Services;
using Common.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class ConnectionManager : Service
    {
        [Inject]
        private readonly ServerOption ServerOptions;

        [Inject]
        private readonly StatusService StatusService;

        [Inject]
        private readonly PlotService PlotService;

        private HubConnection Connection;

        protected override async ValueTask InitializeAsync()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"https://{ServerOptions.PoolHost}:{ServerOptions.ManagerPort}/PHub")
                .WithAutomaticReconnect(new PersistentRetryPolicy())
                .Build();

            Connection.On(PlotterMethods.RequestPlot, () => HandlePlotRequestAsync());

            await Connection.StartAsync();
        }

        public async Task SendStatusUpdateAsync()
        {
            var status = await StatusService.GetStatusAsync();
            await Connection.SendAsync(ServerMethods.Update, status);
        }

        private async Task HandlePlotRequestAsync()
        {
            var remotePlot = await PlotService.ActivateRemotePlotAsync();
            await SendStatusUpdateAsync();
            await Connection.SendAsync(ServerMethods.OfferPlot, remotePlot);
        }

        protected override async ValueTask RunAsync()
        {
            var status = await StatusService.GetStatusAsync();
            await Connection.SendAsync(ServerMethods.Activate, status);
        }
    }
}
