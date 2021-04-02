using ChiaPool.Api;
using ChiaPool.Extensions;
using ChiaPool.Models;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands.Status
{
    [Command("Status", Description = "Get miner status")]
    public class StatusCommand : ChiaCommand
    {
        private readonly ServerApiAccessor ServerAccessor;
        private readonly ClientApiAccessor ClientAccessor;

        public StatusCommand(ServerApiAccessor serverAccessor, ClientApiAccessor clientAccessor)
        {
            ServerAccessor = serverAccessor;
            ClientAccessor = clientAccessor;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var clientStatusTask = ClientAccessor.GetStatusAsync().Try();
            var serverStatusTask = ServerAccessor.GetStatusAsync().Try();

            ClientStatus clientStatus = await clientStatusTask;
            ServerStatus serverStatus = await serverStatusTask;

            await InfoAsync($"Client ");

            if (clientStatus == null)
            {
                await ErrorLineAsync("Offline");
            }
            else
            {
                await SuccessLineAsync("Online");
                await WriteLineAsync();
                await InfoLineAsync($"Currently harvesting {clientStatus.PlotCount} plots");
            }

            await WriteLineAsync();
            await InfoAsync($"Server ");

            if (serverStatus == null)
            {
                await ErrorLineAsync("Offline");
            }
            else
            {
                await SuccessLineAsync("Online");
                await WriteLineAsync();
                await InfoLineAsync($"Currently synced to block height {serverStatus.SyncHeight}");
                await InfoLineAsync($"Peak block height {serverStatus.MaxSyncHeight}");
            }
        }
    }
}
