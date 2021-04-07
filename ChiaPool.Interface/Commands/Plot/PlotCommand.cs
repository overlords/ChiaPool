using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Plot")]
    public sealed class PlotCommand : ChiaCommand
    {
        protected override Task ExecuteAsync(IConsole console)
        {
            return Task.CompletedTask;
        }
    }
}
