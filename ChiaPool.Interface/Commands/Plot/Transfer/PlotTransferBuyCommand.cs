using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot Transfer Buy")]
    public sealed class PlotTransferBuyCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public PlotTransferBuyCommand(MinerApiAccessor minerAccessor)
        {
            MinerAccessor = minerAccessor;
        }

        protected override Task ExecuteAsync(IConsole console)
        {
            return WarnAsync("Not implemented yet");
        }
    }
}
