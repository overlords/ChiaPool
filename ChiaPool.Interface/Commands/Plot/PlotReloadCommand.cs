using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot Reload")]
    public sealed class PlotReloadCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;

        public PlotReloadCommand(MinerApiAccessor minerAccessor)
        {
            MinerAccessor = minerAccessor;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            await InfoLineAsync("Reloading plots...");
            await MinerAccessor.ReloadPlotsAsync();
            await InfoLineAsync("Done");
        }
    }
}
