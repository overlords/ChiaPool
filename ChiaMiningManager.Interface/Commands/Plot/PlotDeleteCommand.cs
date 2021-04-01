using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot Delete", Description = "Deletes a plot file")]
    public class PlotDeleteCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;

        public PlotDeleteCommand(ClientApiAccessor clientAccessor)
        {
            ClientAccessor = clientAccessor;
        }

        [CommandOption("pubkey", 'p', Description = "The public key of the plot file")]
        public string PublicKey { get; set; }

        [CommandOption("file", 'f', Description = "The filename of the plot file")]
        public string FileName { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            if (PublicKey == default && PublicKey == default)
            {
                await ErrorLineAsync("Please specify the public key or file name of the plot");
                return;
            }
            if (FileName != default && FileName != default)
            {
                await ErrorLineAsync("Please specify either the public key or file name of the plot, not both");
                return;
            }
            bool success = false;

            if (PublicKey != default)
            {
                success = await ClientAccessor.DeletePlotByPublicKeyAsync(PublicKey);
            }
            if (FileName != default)
            {
                success = await ClientAccessor.DeletePlotByFileNameAsync(FileName);
            }

            if (!success)
            {
                await WarnLineAsync("Plot not found!");
                return;
            }

            await SuccessLineAsync("Ok");
        }
    }
}
