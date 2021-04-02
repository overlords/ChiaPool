using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot Generate", Description = "Generates a new plot")]
    public class PlotCreateCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;

        public PlotCreateCommand(ClientApiAccessor clientAccessor)
        {
            ClientAccessor = clientAccessor;
        }

        protected override Task ExecuteAsync(IConsole console)
        {
            throw new NotImplementedException();
        }
    }
}
