using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Miner Show", Description = "Retrieves information about a specific miner. Defaults to you")]
    public class MinerShowCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public MinerShowCommand(ClientApiAccessor clientAccessor, ServerApiAccessor serverAccessor)
        {
            ClientAccessor = clientAccessor;
            ServerAccessor = serverAccessor;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var miner = await ClientAccessor.GetCurrentMinerAsync();

            await InfoLineAsync($"[ID]      |   {miner.Id}");
            await InfoLineAsync($"[Name]    |   {miner.Name}");
            await InfoLineAsync($"[PM]      |   {miner.PlotMinutes}");
        }
    }
}
