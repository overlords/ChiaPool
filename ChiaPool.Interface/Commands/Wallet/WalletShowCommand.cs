using ChiaPool.Api;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;

namespace ChiaPool.Commands
{
    [Command("Wallet Show", Description = "Retrieves information about a your or the pools wallet")]
    public class WalletShowCommand : ChiaCommand
    {
        private readonly MinerApiAccessor ClientAccessor;
        private readonly ServerApiAccessor ServerAccessor;

        public WalletShowCommand(MinerApiAccessor clientAccessor, ServerApiAccessor serverAccessor)
        {
            ClientAccessor = clientAccessor;
            ServerAccessor = serverAccessor;
        }

        [CommandOption("pool", 'p', Description = "Show wallet of the entire pool")]
        public bool Total { get; set; }

        protected override async Task ExecuteAsync(IConsole console)
        {
            var wallet = Total
                ? await ServerAccessor.GetPoolWalletAsync()
                : await ClientAccessor.GetCurrentWalletAsync();

            if (Total)
            {
                await InfoLineAsync("[Pool Wallet]");
            }
            else
            {
                await InfoLineAsync($"[Your Wallet : {wallet.Percentage}% of the pool wallet]");
            }

            await InfoLineAsync($"Confirmed Balance   : {wallet.ConfirmedBalance} XCH");
            await InfoLineAsync($"Unconfirmed Balance : {wallet.UnconfirmedBalance} XCH");
            await InfoLineAsync($"Spendable Balance   : {wallet.SpendableBalance} XCH");
            await InfoLineAsync($"Pending Change      : {wallet.PendingChange} XCH");
            await InfoLineAsync($"Maximum Send Amount : {wallet.MaxSendAmount} XCH");
        }
    }
}
