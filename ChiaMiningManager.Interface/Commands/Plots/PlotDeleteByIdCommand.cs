using ChiaMiningManager.Api;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands
{
    [Command("Plots DeleteId", Description = "Deletes a plot by specifying the plot id")]
    public class PlotDeleteByIdCommand : ICommand
    {
        private readonly ClientApiAccessor ApiClient;

        public PlotDeleteByIdCommand(ClientApiAccessor apiClient)
        {
            ApiClient = apiClient;
        }

        [CommandParameter(0, Description = "Id of the plot", Name = "Id")]
        public Guid Id { get; set; }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            bool success = await ApiClient.DeletePlotByIdAsync(Id);

            if (success)
            {
                await console.Output.WriteLineAsync("Ok");
                return;
            }

            await console.Output.WriteLineAsync("Plot not found!");
        }
    }
}
