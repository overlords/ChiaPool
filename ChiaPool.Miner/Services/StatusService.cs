using ChiaPool.Models;
using Common.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    [InitializationPriority(-10)] //Init after everything else 
    public class StatusService : Service, IStatusService<MinerStatus>
    {
        private const int StatusRefreshDelay = 30 * 1000;

        [Inject]
        private readonly ConnectionService ConnectionManager;
        [Inject]
        private readonly PlotService PlotManager;

        private MinerStatus CurrentStatus;

        protected override async ValueTask InitializeAsync()
            => CurrentStatus = await LoadCurrentStatusAsync();

        protected override async ValueTask RunAsync()
        {
            while (true)
            {
                await Task.Delay(StatusRefreshDelay);
                await RefreshStatusAsync();
            }
        }

        public MinerStatus GetCurrentStatus()
            => CurrentStatus;

        public async Task RefreshStatusAsync()
        {
            try
            {
                var newStatus = await LoadCurrentStatusAsync();

                if (CurrentStatus.Equals(newStatus))
                {
                    return;
                }

                CurrentStatus = newStatus;
                await ConnectionManager.SendStatusUpdateAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "There was an error while refreshing status");
            }
        }

        private async Task<MinerStatus> LoadCurrentStatusAsync()
            => new MinerStatus(
                await PlotManager.GetPlotCountAsync()
            );
    }
}
