using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Pool Show", Description = "Retrieves some information about the pool")]
    public class PoolShowCommand : ChiaCommand
    {
        private readonly ServerApiAccessor ServerAccessor;

        public PoolShowCommand(ServerApiAccessor serverAccessor)
        {
            ServerAccessor = serverAccessor;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var poolInfo = await ServerAccessor.GetPoolInfoAsync();

            await InfoLineAsync($"[Info for Pool {poolInfo.Name}]");

            await InfoLineAsync($"Total  Miners     |   {poolInfo.TotalMinerCount}");
            await InfoLineAsync($"Active Miners     |   {poolInfo.ActiveMinerCount}");
            await InfoLineAsync($"Total  Plots      |   {poolInfo.TotalPlotCount}");
            await InfoLineAsync($"Active Plots      |   {poolInfo.ActivePlotCount}");
            await InfoLineAsync($"Total Pool Size   |   {103 * poolInfo.TotalPlotCount} GB");
            await InfoLineAsync($"PM Mining Speed   |   {poolInfo.ActivePlotCount / 60d} PM / second");
        }
    }
}
