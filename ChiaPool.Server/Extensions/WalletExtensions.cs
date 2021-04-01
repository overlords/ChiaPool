using Chia.NET.Models;
using System;

namespace ChiaPool.Extensions
{
    public static class WalletExtensions
    {
        public static Wallet GetFraction(this Wallet wallet, double fraction)
        {
            return new Wallet()
            {
                Id = wallet.Id,
                ConfirmedBalance = wallet.ConfirmedBalance * fraction,
                UnconfirmedBalance = wallet.UnconfirmedBalance * fraction,
                SpendableBalance = wallet.SpendableBalance * fraction,
                MaxSendAmount = wallet.MaxSendAmount * fraction,
                PendingChange = wallet.PendingChange * fraction,
                Percentage = Math.Round(100d * fraction, 2)
            };
        }
    }
}
