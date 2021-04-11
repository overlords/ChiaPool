using ChiaPool.Models;
using Common.Services;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    [InitializationPriority(-10)]
    public class StatusService : Service, IStatusService<PlotterStatus>
    {
        private const int StatusRefreshDelay = 30 * 1000;

        [Inject]
        private readonly PlotService PlotService;
        [Inject]
        private readonly PlotStorageService PlotStorageService;
        [Inject]
        private readonly ConnectionManager ConnectionManager;

        private PlotterStatus CurrentStatus;

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

        public PlotterStatus GetCurrentStatus()
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

        private async Task<PlotterStatus> LoadCurrentStatusAsync()
            => new PlotterStatus()
            {
                Capacity = PlotStorageService.GetCapacity(),
                PlotsAvailable = await PlotService.GetAvailablePlotCountAsync(),
            };
    }
}
