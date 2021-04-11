using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Wallet")]
    public sealed class WalletCommand : ChiaCommand
    {
        protected override Task ExecuteAsync(IConsole console)
        {
            return Task.CompletedTask;
        }
    }
}
