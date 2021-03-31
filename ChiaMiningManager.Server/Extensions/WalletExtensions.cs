using Chia.NET.Models;

namespace ChiaMiningManager.Extensions
{
    public static class WalletExtensions
    {
        public static Wallet MultiplyBy(this Wallet wallet, double factor)
        {
            return new Wallet()
            {
                Id = wallet.Id,
                ConfirmedBalance = wallet.ConfirmedBalance * factor,
                UnconfirmedBalance = wallet.UnconfirmedBalance * factor,
                SpendableBalance = wallet.SpendableBalance * factor,
                MaxSendAmount = wallet.MaxSendAmount * factor,
                PendingChange = wallet.PendingChange * factor,
            };
        }
    }
}
