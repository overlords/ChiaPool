using Common.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services.Plot
{
    public class PlotGenerationService : Service
    {
        [Inject]
        private readonly PlotStorageService PlotStorageService;



        public PlotGenerationService()
        {

        }

        protected override ValueTask RunAsync()
        {
            PerformPlotShifting();
            PerformPlotGeneration();

            return ValueTask.CompletedTask;
        }

        public void PerformPlotShifting()
        {
            var plottableLocations = PlotStorageService.StorageLocations.Where(x => x.SupportsPlotting);
            var nonPlottableLocations = PlotStorageService.StorageLocations.Where(x => !x.SupportsPlotting);


        }

        public void PerformPlotGeneration()
        {

        }
    }
}
