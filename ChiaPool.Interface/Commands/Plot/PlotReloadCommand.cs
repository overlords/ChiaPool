using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot Reload")]
    public sealed class PlotReloadCommand : ChiaCommand
    {
        private readonly MinerApiAccessor ClientAccessor;

        public PlotReloadCommand(MinerApiAccessor clientAccessor)
        {
            ClientAccessor = clientAccessor;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            await InfoLineAsync("Reloading plots...");
            await ClientAccessor.ReloadPlotsAsync();
            await InfoLineAsync("Done");
        }
    }
}
