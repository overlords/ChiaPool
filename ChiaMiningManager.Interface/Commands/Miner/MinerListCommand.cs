using ChiaMiningManager.Api;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands.Miner
{
    [Command("Miner List", Description = "Lists all miners in the pool")]
    public class MinerListCommand : ICommand
    {
        private readonly ServerApiAccessor ApiClient;

        public MinerListCommand(ServerApiAccessor apiClient)
        {
            ApiClient = apiClient;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var miners = await ApiClient.GetMinersAsync();

            if (miners.Count == 0)
            {
                await console.Output.WriteLineAsync("No miners found");
                return;
            }

            await console.Output.WriteLineAsync("Id    Name    PM");

            foreach(var miner in miners)
            {
                await console.Output.WriteLineAsync($"{miner.Id}    {miner.Name}    {miner.PlotMinutes}");
            }
        }
    }
}
