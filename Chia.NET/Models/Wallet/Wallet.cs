using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class Wallet
    {
        [JsonPropertyName("wallet_id")]
        public int Id { get; }

        [JsonPropertyName("confirmed_wallet_balance")]
        public double ConfirmedBalance { get; }

        [JsonPropertyName("unconfirmed_wallet_balance")]
        public double UnconfirmedBalance { get; }

        [JsonPropertyName("spendable_balance")]
        public double SpendableBalance { get; }

        [JsonPropertyName("max_send_amount")]
        public double MaxSendAmount { get; }

        [JsonPropertyName("pending_change")]
        public double PendingChange { get; }

        [JsonConstructor]
        internal Wallet()
        {
        }
    }
}
