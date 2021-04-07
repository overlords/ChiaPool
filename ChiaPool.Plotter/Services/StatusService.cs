using ChiaPool.Models;
using ChiaPool.Plotter.Services;
using Common.Services;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class StatusService : Service
    {
        private readonly PlotService PlotService;

        public async Task<PlotterStatus> GetStatusAsync()
            => new PlotterStatus()
            {
                Capacity = PlotService.GetPlotCapacity(),
                PlotsAvailable = await PlotService.GetAvailablePlotCountAsync(),
            };
    }
}
