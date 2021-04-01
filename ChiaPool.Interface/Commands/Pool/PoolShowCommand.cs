using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands.Pool
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

            await InfoAsync($"[Info for Pool {poolInfo.Name}]");

            await InfoAsync($"Total  Miners     |   {poolInfo.TotalMinerCount}");
            await InfoAsync($"Active Miners     |   {poolInfo.ActiveMinerCount}");
            await InfoAsync($"Total  Plots      |   {poolInfo.TotalPlotCount}");
            await InfoAsync($"Active Plots      |   {poolInfo.ActivePlotCount}");
            await InfoAsync($"Total Pool Size   |   {103 * poolInfo.TotalPlotCount} GB");
            await InfoAsync($"PM Mining Speed   |   {poolInfo.ActivePlotCount / 60d} PM / second");
        }
    }
}
