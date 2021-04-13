using ChiaPool.Clients;
using ChiaPool.Hubs;
using ChiaPool.Models;
using ChiaPool.Services.Abstraction;
using Common.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class PlotterService : Service, IPlotOfferHandler
    {
        private const int PlotResponseTimeout = 10 * 1000;
        private const int PlotBaseCost = 60 * 10; //10 Plot Hours
        private const int PlotDeadLineHourCost = 2 * 60;

        [Inject]
        private readonly IHubContext<PlotterHub> HubContext;

        private readonly ConcurrentDictionary<long, PlotterStatus> ActivePlotters;

        private TaskCompletionSource<RemotePlot> PlotOfferCallback;
        private long PlotOfferPlotterId;

        public PlotterService()
        {
            ActivePlotters = new ConcurrentDictionary<long, PlotterStatus>();
        }

        public async Task<int> GetPlotterCount()
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();

            return await dbContext.Plotters.CountAsync();
        }
        public int GetActivePlotterCount()
            => ActivePlotters.Count;

        public int GetPlottingCapacity()
            => ActivePlotters.Sum(x => x.Value.Capacity);
        public int GetAvailablePlotCount()
            => ActivePlotters.Sum(x => x.Value.PlotsAvailable);

        public PlotterInfo GetPlotterInfo(Plotter plotter)
           => ActivePlotters.TryGetValue(plotter.Id, out var plotterStatus)
               ? new PlotterInfo(plotter.Id, true, plotterStatus.Capacity, plotterStatus.PlotsAvailable , plotter.Name, plotter.Earnings, plotter.OwnerId)
               : new PlotterInfo(plotter.Id, false, -1, -1, plotter.Name, plotter.Earnings, plotter.OwnerId);

        public Task ActivatePlotterAsync(long plotterId, PlotterStatus status)
            => !ActivePlotters.TryAdd(plotterId, status)
                ? throw new InvalidOperationException("Cannot activate active plotter")
                : Task.CompletedTask;
        public Task UpdatePlotterAsync(long plotterId, PlotterStatus status)
            => !ActivePlotters.TryUpdate(plotterId, status, status)
                ? throw new InvalidOperationException("Cannot update inactive plotter")
                : Task.CompletedTask;
        public Task DeactivatePlotter(long plotterId)
            => !ActivePlotters.TryRemove(plotterId, out _)
                ? throw new InvalidOperationException("Cannot deactivate inactive plotter")
                : Task.CompletedTask;

        public long GetPlotPrice(int deadlineHours)
        {
            int availablePlots = GetAvailablePlotCount();

            if (availablePlots == 0)
            {
                return -1;
            }

            int capacity = GetPlottingCapacity();

            return (PlotBaseCost * capacity / availablePlots) + (deadlineHours * PlotDeadLineHourCost);
        }

        public long? GetSuitablePlotterId()
            => ActivePlotters
                .OrderBy(x => x.Value.Capacity - x.Value.PlotsAvailable)
                .Where(x => x.Value.PlotsAvailable > 0)
                .Select(x => x.Key)
                .FirstOrDefault();

        public async Task<PlotTransfer> TryRequestPlotTransferAsync(long minerId, long plotterId, int deadlineHours)
        {
            PlotOfferCallback = new TaskCompletionSource<RemotePlot>();
            PlotOfferPlotterId = plotterId;

            await HubContext.Clients.User($"{plotterId}")
                .SendAsync(PlotterMethods.RequestPlot);

            var result = await Task.WhenAny(PlotOfferCallback.Task, Task.Delay(PlotResponseTimeout));

            if (result != PlotOfferCallback.Task)
            {
                throw new TimeoutException($"Plotter did not respond after {PlotResponseTimeout} ms");
            }

            var remotePlot = await PlotOfferCallback.Task;
            return new PlotTransfer(plotterId, remotePlot.PlotId, minerId, GetPlotPrice(deadlineHours), remotePlot.DownloadAddress, deadlineHours);
        }

        ValueTask IPlotOfferHandler.HandlePlotOfferAsync(RemotePlot plot, long plotterId)
        {
            if (PlotOfferPlotterId == plotterId && PlotOfferCallback != null)
            {
                PlotOfferCallback.SetResult(plot);
            }
            return ValueTask.CompletedTask;
        }
    }
}
