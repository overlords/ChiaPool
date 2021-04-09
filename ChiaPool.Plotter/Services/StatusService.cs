using ChiaPool.Models;
using Common.Services;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class StatusService : Service
    {
        private readonly PlotService PlotService;
        private readonly PlotStorageService PlotStorageService;

        public async Task<PlotterStatus> GetStatusAsync()
            => new PlotterStatus()
            {
                Capacity = PlotStorageService.GetCapacity(),
                PlotsAvailable = await PlotService.GetAvailablePlotCountAsync(),
            };
    }
}
