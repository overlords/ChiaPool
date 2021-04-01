using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Wallet Show", Description = "Retrieves information about a your or the pools wallet")]
    public class WalletShowCommand : ChiaCommand
    {
        private readonly ClientApiAccessor ClientAccessor;

        public WalletShowCommand(ClientApiAccessor clientAccessor)
        {
            ClientAccessor = clientAccessor;
        }

        [CommandOption("pool", 'p', Description = "Show wallet of the entire pool")]
        public bool Total { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var wallet = Total
                ? await ClientAccessor.GetPoolWalletAsync()
                : await ClientAccessor.GetCurrentWalletAsync();

            if (Total)
            {
                await InfoAsync("[Pool Wallet]");
            }
            else
            {
                await InfoAsync($"[Your Wallet : {wallet.Percentage}% of the pool wallet]");
            }

            await InfoAsync($"Confirmed Balance   : {wallet.ConfirmedBalance}   XCH");
            await InfoAsync($"Unconfirmed Balance : {wallet.UnconfirmedBalance} XCH");
            await InfoAsync($"Spendable Balance   : {wallet.SpendableBalance}   XCH");
            await InfoAsync($"Pending Change      : {wallet.PendingChange}      XCH");
            await InfoAsync($"Maximum Send Amount : {wallet.MaxSendAmount}      XCH");
        }
    }
}
