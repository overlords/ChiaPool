using ChiaMiningManager.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands
{
    [Command("Plot List", Description = "Lists all plots of your harvester")]
    public class PlotsListCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;

        public PlotsListCommand(ClientApiAccessor apiClient)
        {
            ClientAccessor = apiClient;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var plots = await ClientAccessor.GetPlotsAsync();

            if (plots.Length == 0)
            {
                await WarnLineAsync("There are no plots on this miner!");
                return;
            }

            int publicKeyLength = plots.Max(x => x.PublicKey.Length) - 6;
            int fileNameLength = plots.Max(x => x.FileName.Length) - 5;

            await InfoLineAsync($"Public Key{Space(publicKeyLength)}File Name{Space(fileNameLength)}Minutes");

            foreach (var plot in plots)
            {
                await WriteLineAsync($"{plot.PublicKey}    {plot.FileName}    {plot.Minutes}");
            }
        }
    }
}
