using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Wallet Payout", Description = "Payout your XCH to your personal wallet/address")]
    public sealed class WalletPayoutCommand : ChiaCommand
    {
        protected override async Task ExecuteAsync(IConsole console)
        {
            await WarnLineAsync("Transactions are not yet supported!");
            await WarnLineAsync("The first transaction on the chia blockchain will be on the 30. april 2021");
            await WarnLineAsync("From there on it will take a while until payout will be supported!");
        }
    }
}
