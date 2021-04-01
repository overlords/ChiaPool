using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Miner List", Description = "Lists all miners in the pool")]
    public class MinerListCommand : ChiaCommand
    {
        private readonly ServerApiAccessor ServerAccessor;

        public MinerListCommand(ServerApiAccessor apiClient)
        {
            ServerAccessor = apiClient;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var miners = await ServerAccessor.GetMinersAsync();

            if (miners.Count == 0)
            {
                await WarnLineAsync("No miners found");
                return;
            }

            int idLength = miners.Max(x => x.Id.ToString().Length) + 2;
            int nameLength = miners.Max(x => x.Name.Length);

            long totalPM = miners.Sum(x => x.PlotMinutes);
            await InfoLineAsync($"Total PM mined in the pool: {totalPM}");
            await WriteLineAsync();
            await InfoLineAsync($"Id{Space(idLength)}Name{Space(nameLength)}PM");

            foreach (var miner in miners)
            {
                await WriteLineAsync($"{miner.Id}    {miner.Name}    {miner.PlotMinutes}");
            }
        }
    }
}
