using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Wallet Info", Description = "Show the pool wallet info")]
    public class WalletInfoCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;

        public WalletInfoCommand(ClientApiAccessor clientAccessor)
        {
            ClientAccessor = clientAccessor;
        }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var wallet = await ClientAccessor.GetWalletBalanceAsync();

            await console.Output.WriteLineAsync($"Confirmed Balance   : {wallet.ConfirmedBalance} XCH");
            await console.Output.WriteLineAsync($"Unconfirmed Balance : {wallet.UnconfirmedBalance} XCH");
            await console.Output.WriteLineAsync($"Spendable Balance   : {wallet.SpendableBalance} XCH");
            await console.Output.WriteLineAsync($"Pending Change      : {wallet.PendingChange} XCH");
            await console.Output.WriteLineAsync($"Maximum Send Amount : {wallet.MaxSendAmount} XCH");
        }
    }
}
