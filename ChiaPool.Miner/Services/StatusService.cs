using ChiaPool.Models;
using Common.Services;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    [InitializationPriority(-10)] //Init after everything else 
    public class StatusService : Service, IStatusService<MinerStatus>
    {
        private const int StatusRefreshDelay = 30 * 1000;

        [Inject]
        private readonly ConnectionManager ConnectionManager;
        [Inject]
        private readonly PlotManager PlotManager;

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
            var newStatus = await LoadCurrentStatusAsync();

            if (CurrentStatus.Equals(newStatus))
            {
                return;
            }

            CurrentStatus = newStatus;
            await ConnectionManager.SendStatusUpdateAsync();
        }

        private async Task<MinerStatus> LoadCurrentStatusAsync()
            => new MinerStatus(
                await PlotManager.GetPlotCountAsync()
            );
    }
}
