using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot Transfer Buy", Description = "Buy a plot transfer for the current cost")]
    public sealed class PlotTransferBuyCommand : ChiaCommand
    {
        private readonly MinerApiAccessor MinerAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public PlotTransferBuyCommand(MinerApiAccessor minerAccessor, ServerApiAccessor serverAccessor)
        {
            MinerAccessor = minerAccessor;
            ServerAccessor = serverAccessor;
        }

        [CommandOption("time", 't', Description = "Amount of hours you want to pay for until the download expires")]
        public int DeadlineHours { get; set; } = 12;


        protected override async Task ExecuteAsync(IConsole console)
        {
            long cost = await ServerAccessor.GetPlotTransferCostAsync(DeadlineHours);

            if (cost == -1)
            {
                await WarnLineAsync("There are no plots available at the moment!");
                await WarnLineAsync("Try again later...");
                return;
            }

            await WarnLineAsync($"You are about to buy a plot tranfer for {cost} PM.");
            await WarnLineAsync("Do you want to proceed? (y/n)");

            if (await ReadKeyAsync() != 'y')
            {
                await WriteLineAsync("Aborted!");
                return;
            }

            await ErrorLineAsync("Not implemented yet");
        }
    }
}
