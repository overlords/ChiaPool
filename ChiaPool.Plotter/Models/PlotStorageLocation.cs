using System.IO;
using System.Linq;

namespace ChiaPool.Models
{
    public class PlotStorageLocation
    {
        private PlotDirectory PlotDirectory;

        public string RootPath => PlotDirectory.Path;
        public string PlotPath => Path.Combine(PlotDirectory.Path, "Plots");
        public string CachePath => Path.Combine(PlotDirectory.Path, "Cache");

        public bool SupportsPlotting => PlotDirectory.Size > Constants.PlotGenerationSize && PlotDirectory.Parallelism > 0;
        public int Capacity => (int)(PlotDirectory.Size / Constants.PlotSize);
        public int Size => PlotDirectory.Size;
        public int Parallelism => PlotDirectory.Parallelism;
        public int FreeCapacity => Capacity - CountPlotFiles();

        public PlotStorageLocation(PlotDirectory plotDirectory)
        {
            PlotDirectory = plotDirectory;

            FlushNonPlotFiles();

            Directory.CreateDirectory(RootPath);
            Directory.CreateDirectory(PlotPath);
            Directory.CreateDirectory(CachePath);
        }

        public int CountPlotFiles()
            => Directory.GetFiles(PlotPath, "", SearchOption.AllDirectories)
                        .Where(x => x.EndsWith(".plot"))
                        .Count();
        public bool ContainsPlot(long plotId)
            => Directory.Exists(GetPlotDirectory(plotId));
        public void DeletePlot(long plotId)
            => Directory.Delete(GetPlotDirectory(plotId));
        public string GetPlotPath(long plotId)
            => Directory.GetFiles(GetPlotDirectory(plotId)).First();

        private void FlushNonPlotFiles()
        {
            foreach (var file in Directory.EnumerateFiles(RootPath, "", SearchOption.AllDirectories))
            {
                if (file.EndsWith(".plot"))
                {
                    continue;
                }

                File.Delete(file);
            }
        }

        private string GetPlotDirectory(long plotId)
            => Path.Combine(PlotPath, $"Plot_{plotId}");
    }
}
