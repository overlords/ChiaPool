using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("User")]
    public sealed class UserCommand : ChiaCommand
    {
        protected override Task ExecuteAsync(IConsole console)
        {
            return Task.CompletedTask;
        }
    }
}
