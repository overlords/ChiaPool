using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot List", Description = "Lists all plots of your harvester")]
    public class PlotListCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;

        public PlotListCommand(MinerApiAccessor apiClient)
        {
            MinerAccessor = apiClient;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var plots = await MinerAccessor.GetPlotsAsync();

            if (plots.Length == 0)
            {
                await WarnLineAsync("There are no plots on this miner!");
                return;
            }

            int publicKeyLength = plots.Max(x => x.PublicKey.Length) - 6;
            int fileNameLength = plots.Max(x => x.FileName.Length) - 5;

            await InfoLineAsync($"Public Key{Space(publicKeyLength)}File Name");

            foreach (var plot in plots)
            {
                await WriteLineAsync($"{plot.PublicKey}    {plot.FileName.Split('/').Last()}");
            }
        }
    }
}
