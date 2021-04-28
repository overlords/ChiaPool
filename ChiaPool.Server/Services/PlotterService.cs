using ChiaPool.Clients;
using ChiaPool.Hubs;
using ChiaPool.Models;
using ChiaPool.Services.Abstraction;
using Common.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        [Inject]
        private readonly UserService UserService;

        private readonly Dictionary<long, PlotterActivation> ActivePlotters;
        private readonly SemaphoreSlim ActivePlottersLock;

        private TaskCompletionSource<RemotePlot> PlotOfferCallback;
        private long PlotOfferPlotterId;

        public PlotterService()
        {
            ActivePlotters = new Dictionary<long, PlotterActivation>();
            ActivePlottersLock = new SemaphoreSlim(1, 1);
        }

        public async Task<int> GetTotalPlotterCountAsync()
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();

            return await dbContext.Plotters.CountAsync();
        }
        public int GetActivePlotterCount()
            => ActivePlotters.Count;

        public int GetPlottingCapacity()
            => ActivePlotters.Sum(x => x.Value.Status.Capacity);
        public int GetAvailablePlotCount()
            => ActivePlotters.Sum(x => x.Value.Status.PlotsAvailable);

        public PlotterInfo GetPlotterInfo(Plotter plotter)
           => ActivePlotters.TryGetValue(plotter.Id, out var plotterStatus)
               ? new PlotterInfo(plotter.Id, true, plotterStatus.Status.Capacity, plotterStatus.Status.PlotsAvailable, plotter.Name, plotter.Earnings, plotter.OwnerId)
               : new PlotterInfo(plotter.Id, false, -1, -1, plotter.Name, plotter.Earnings, plotter.OwnerId);

        public async Task<PlotterActivationResult> ActivatePlotterAsync(string connectionId, long plotterId, PlotterStatus status)
        {
            await ActivePlottersLock.WaitAsync();
            try
            {
                var activation = new PlotterActivation(connectionId, status);
                if (!ActivePlotters.TryAdd(plotterId, activation))
                {
                    return PlotterActivationResult.FromFailed("There already is a active connection from this plotter!");
                }

                Logger.LogInformation($"Activated plotter [{plotterId}]");
                long userId = await UserService.GetOwnerIdFromPlotterId(plotterId);
                return PlotterActivationResult.FromSuccess(userId);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "There was an exception while activating a plotter!");
                return PlotterActivationResult.FromFailed("An unknown error occurred!");
            }
            finally
            {
                ActivePlottersLock.Release();
            }
        }
        public async Task<PlotterUpdateResult> UpdatePlotterAsync(string connectionId, long plotterId, PlotterStatus status)
        {
            await ActivePlottersLock.WaitAsync();

            try
            {
                if (!ActivePlotters.TryGetValue(plotterId, out var oldValue))
                {
                    throw new InvalidOperationException("Cannot update inactive plotter");
                }
                if (oldValue.ConnectionId != connectionId)
                {
                    throw new InvalidOperationException("Cannot update active plotter from different connection");
                }

                oldValue.Status = status;
                ActivePlotters[plotterId] = oldValue;
                Logger.LogInformation($"Updated plotter [{plotterId}]");
                return PlotterUpdateResult.FromSuccess();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "There was an excpetion while updating a plotter!");
                return PlotterUpdateResult.FromFailed("An unknown error occurred!");
            }
            finally
            {
                ActivePlottersLock.Release();
            }
        }
        public async Task DeactivatePlotterAsync(string connectionId, long plotterId)
        {
            await ActivePlottersLock.WaitAsync();

            try
            {
                if (!ActivePlotters.TryGetValue(plotterId, out var oldValue))
                {
                    throw new InvalidOperationException("Cannot deactivate inactive plotter");
                }
                if (oldValue.ConnectionId != connectionId)
                {
                    Logger.LogWarning("Cannot deactivate plotter from different connection");
                    return;
                    //throw new InvalidOperationException("Cannot deactivate plotter from different connection");
                }

                ActivePlotters.Remove(plotterId);
                Logger.LogInformation($"Deactivated plotter [{plotterId}]");
            }
            finally
            {
                ActivePlottersLock.Release();
            }
        }

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
                .OrderBy(x => x.Value.Status.Capacity - x.Value.Status.PlotsAvailable)
                .Where(x => x.Value.Status.PlotsAvailable > 0)
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
