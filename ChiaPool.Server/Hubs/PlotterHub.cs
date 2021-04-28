using ChiaPool.Clients;
using ChiaPool.Models;
using ChiaPool.Services;
using ChiaPool.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Hubs
{
    [Authorize(AuthenticationSchemes = "Plotter")]
    public class PlotterHub : Hub
    {
        private readonly PlotterService PlotterService;

        public PlotterHub(PlotterService plotterService)
        {
            PlotterService = plotterService;
        }

        [HubMethodName(PlotterHubMethods.Activate)]
        public Task<MinerActivationResult> ActivateAsync(PlotterStatus status)
        {
            long plotterId = long.Parse(Context.UserIdentifier);
            return PlotterService.ActivatePlotterAsync(Context.ConnectionId, plotterId, status);
        }
        [HubMethodName(PlotterHubMethods.Update)]
        public async Task UpdateAsync(PlotterStatus status)
        {
            long plotterId = long.Parse(Context.UserIdentifier);
            await PlotterService.UpdatePlotterAsync(Context.ConnectionId, plotterId, status);
        }

        [HubMethodName(PlotterHubMethods.OfferPlot)]
        public async Task OfferPlotAsync(RemotePlot plot)
        {
            long plotterId = long.Parse(Context.UserIdentifier);
            await ((IPlotOfferHandler)PlotterService).HandlePlotOfferAsync(plot, plotterId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            long plotterId = long.Parse(Context.UserIdentifier);
            await PlotterService.DeactivatePlotterAsync(Context.ConnectionId, plotterId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
