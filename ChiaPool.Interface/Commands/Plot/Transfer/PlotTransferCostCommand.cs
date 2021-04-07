using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot Transfer Cost")]
    public sealed class PlotTransferCostCommand : ChiaCommand
    {
        private readonly ServerApiAccessor ServerAccessor;

        public PlotTransferCostCommand(ServerApiAccessor serverAccessor)
        {
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

            await InfoLineAsync("[Plot Cost]");
            await InfoAsync("Hours     |    ");
            await WriteLineAsync(DeadlineHours.ToString());
            await InfoAsync("PM Cost   |    ");
            await WriteLineAsync(cost.ToString());
        }
    }
}
