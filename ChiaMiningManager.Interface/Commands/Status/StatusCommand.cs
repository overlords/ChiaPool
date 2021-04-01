using ChiaMiningManager.Api;
using ChiaMiningManager.Extensions;
using ChiaMiningManager.Models;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands.Status
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
                await WriteLineAsync();
            }

            await InfoAsync($"Server ");

            if (serverStatus == null)
            {
                await ErrorLineAsync("Offline");
            }
            else
            {
                await SuccessLineAsync("Online");
            }

            console.ResetColor();
        }
    }
}
