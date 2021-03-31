using ChiaMiningManager.Api;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaMiningManager.Commands
{
    [Command("Info Wallet", Description = "Show the pool wallet info")]
    public class WalletInfoCommand : ICommand
    {
        private readonly ServerApiAccessor ServerAccessor;

        public WalletInfoCommand(ServerApiAccessor apiClient)
        {
            ServerAccessor = apiClient;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var wallet = await ServerAccessor.GetWalletBalanceAsync();

            await console.Output.WriteLineAsync($"Confirmed Balance   : {wallet.ConfirmedBalance} XCH");
            await console.Output.WriteLineAsync($"Unconfirmed Balance : {wallet.UnconfirmedBalance} XCH");
            await console.Output.WriteLineAsync($"Spendable Balance   : {wallet.SpendableBalance} XCH");
            await console.Output.WriteLineAsync($"Pending Change      : {wallet.PendingChange} XCH");
            await console.Output.WriteLineAsync($"Maximum Send Amount : {wallet.MaxSendAmount} XCH");
        }
    }
}
