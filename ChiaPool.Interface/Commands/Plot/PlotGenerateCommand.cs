using ChiaPool.Api;
using ChiaPool.Services;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot Generate", Description = "Generates a new plot")]
    public class PlotGenerateCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;
        private readonly ConfigurationService ConfigurationService;

        public PlotGenerateCommand(ClientApiAccessor clientAccessor, ConfigurationService configurationService)
        {
            ClientAccessor = clientAccessor;
            ConfigurationService = configurationService;
        }

        public int Size { get; set; } = 32;

        [CommandOption("path", 'p', Description = "Path of the generated plot. Requires at least 100GB")]
        public string Path { get; set; }

        [CommandOption("cache", 'c', Description = "Path of the temporary files. Requires at least 400GB")]
        public string CachePath { get; set; }

        [CommandOption("buckets", 'u', Description = "Number of buckets. Defaults to 128")]
        public int BucketCount { get; set; }

        [CommandOption("buffer", 'b', Description = "Megabytes for sort / plot buffer. Defaults to 4608")]
        public int BufferSize { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var volumes = await ConfigurationService.GetVolumesAsync();




            throw new NotImplementedException();
        }
    }
}
