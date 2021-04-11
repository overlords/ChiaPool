using ChiaPool.Configuration;
using ChiaPool.Models;
using Common.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class PlotStorageService : Service
    {
        [Inject]
        private readonly StorageOption StorageOptions;

        public List<PlotStorageLocation> StorageLocations { get; }

        public PlotStorageService()
        {
            StorageLocations = new List<PlotStorageLocation>();
        }

        protected override ValueTask InitializeAsync()
        {
            foreach (var plotDirectory in StorageOptions.PlotDirectories)
            {
                if (plotDirectory.Size < Constants.PlotSize)
                {
                    Logger.LogWarning($"Plot directory {plotDirectory.Path} is too small to store a plot.\n" +
                                      $"Consider increasing the size to at least {Constants.PlotSize} GB");
                    continue;
                }
                double unusedSpace = plotDirectory.Size % Constants.PlotSize;
                if (unusedSpace > 10)
                {
                    Logger.LogWarning($"Plot directory {plotDirectory.Path} has {unusedSpace} GB unusable space.\n" +
                        $"Either increase it to fit another {Constants.PlotSize} GB or shrink it by the amount of unused space");
                }

                var storageLocation = new PlotStorageLocation(plotDirectory);
                StorageLocations.Add(storageLocation);
            }

            return ValueTask.CompletedTask;
        }

        public int GetCapacity()
            => StorageLocations.Sum(x => x.Capacity);

        public void DeletePlot(long plotId)
        {
            var directoryService = StorageLocations.FirstOrDefault(x => x.ContainsPlot(plotId));

            if (directoryService == null)
            {
                throw new FileNotFoundException($"Plot file with id {plotId} could not be found in storage!");
            }

            directoryService.DeletePlot(plotId);
        }

        protected override ValueTask RunAsync()
        {
            return base.RunAsync();
        }
    }
}
