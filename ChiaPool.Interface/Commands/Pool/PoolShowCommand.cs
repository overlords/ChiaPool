using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
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

            await InfoLineAsync($"Total  Miners       |   {poolInfo.TotalMinerCount}");
            await InfoLineAsync($"Active Miners       |   {poolInfo.ActiveMinerCount}");
            await WriteLineAsync();
            await InfoLineAsync($"Total Plotters      |   {poolInfo.TotalPlotterCount}");
            await InfoLineAsync($"Active Plotters     |   {poolInfo.ActivePlotterCount}");
            await WriteLineAsync();
            await InfoLineAsync($"Mined Plots         |   {poolInfo.MinerPlots}");
            await InfoLineAsync($"Downloadable Plots  |   {poolInfo.PlotterPlots}");
            await WriteLineAsync();
            await InfoLineAsync($"Total Pool Size     |   {106 * (poolInfo.PlotterPlots + poolInfo.MinerPlots)} GB");
            await InfoLineAsync($"PM Mining Speed     |   {Math.Round(poolInfo.MinerPlots / 60d, 2)} PM / second");
        }
    }
}
