using ChiaPool.Models;
using Common.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class StorageOption : Option
    {
        private string RawPlotDirectories { get; set; } = Environment.GetEnvironmentVariable("plot_dirs");
        public PlotDirectory[] PlotDirectories { get; private set; }

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            var plotDirectories = new List<PlotDirectory>();

            foreach (string rawPlotDirectory in RawPlotDirectories.Split(';'))
            {
                if (!PlotDirectory.TryParse(rawPlotDirectory, out var plotDirectory))
                {
                    return ValueTask.FromResult(ValidationResult.Failed($"Failed to parse plot directory: {rawPlotDirectory}"));
                }

                plotDirectories.Add(plotDirectory);
            }

            PlotDirectories = plotDirectories.ToArray();
            return ValueTask.FromResult(ValidationResult.Success);
        }
    }
}
