using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Log")]
    public class LogCommand : ChiaCommand
    {
        protected override Task ExecuteAsync(IConsole console)
        {
            return Task.CompletedTask;
        }
    }

}
