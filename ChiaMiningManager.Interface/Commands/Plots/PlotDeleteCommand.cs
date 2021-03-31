//using ChiaMiningManager.Api;
//using CliFx;
//using CliFx.Attributes;
//using CliFx.Infrastructure;
//using System;
//using System.Threading.Tasks;

//namespace ChiaMiningManager.Commands
//{
//    [Command("Plot Delete", Description = "Deletes a plot file")]
//    public class PlotDeleteCommand : ICommand
//    {
//        private readonly ClientApiAccessor ApiClient;

//        public PlotDeleteCommand(ClientApiAccessor apiClient)
//        {
//            ApiClient = apiClient;
//        }

//        [CommandOption("id", 'i', Description = "The id of the plot file")]
//        public Guid Id { get; set; }

//        [CommandOption("id", 'i', Description = "The name of the plot file")]
//        public string { get; set; }

//        public async ValueTask ExecuteAsync(IConsole console)
//        {
//            bool success = await ApiClient.DeletePlotByIdAsync(Id);

//            if (success)
//            {
//                await console.Output.WriteLineAsync("Ok");
//                return;
//            }

//            await console.Output.WriteLineAsync("Plot not found!");
//        }
//    }
//}
