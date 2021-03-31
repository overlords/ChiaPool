using ChiaMiningManager.Api;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands
{
    [Command("Plot List", Description = "Lists all plots of your harvester")]
    public class PlotsListCommand : ICommand
    {
        private readonly ClientApiAccessor ApiClient;

        public PlotsListCommand(ClientApiAccessor apiClient)
        {
            ApiClient = apiClient;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var plots = await ApiClient.GetPlotsAsync();

            if (plots.Length == 0)
            {
                await console.Output.WriteLineAsync("There are no plots on this miner!");
                return;
            }

            int publicKeyLength = plots.Max(x => x.PublicKey.Length) - 6;

            await console.Output.WriteLineAsync($"Public Key{GetWhiteSpace(publicKeyLength)}File Path");

            foreach (var plot in plots)
            {
                await console.Output.WriteLineAsync($"{plot.PublicKey}    {plot.FileName}");
            }
        }

        private string GetWhiteSpace(int length)
            => new string(' ', length);
    }
}
