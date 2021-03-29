using ChiaMiningManager.Api;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands
{
    [Command("Plots List")]
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

            if (plots.Count == 0)
            {
                await console.Output.WriteLineAsync("There are no plots on this miner!");
                return;
            }

            await console.Output.WriteLineAsync($"Id    Name    Minutes");

            foreach(var plot in plots)
            {
                await console.Output.WriteLineAsync($"{plot.Id} {plot.Name} {plot.Minutes}");
            }
        }
    }
}
