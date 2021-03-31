using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class Wallet
    {
        [JsonPropertyName("wallet_id")]
        public int Id { get; init; }

        [JsonPropertyName("confirmed_wallet_balance")]
        public double ConfirmedBalance { get; init; }

        [JsonPropertyName("unconfirmed_wallet_balance")]
        public double UnconfirmedBalance { get; init; }

        [JsonPropertyName("spendable_balance")]
        public double SpendableBalance { get; init; }

        [JsonPropertyName("max_send_amount")]
        public double MaxSendAmount { get; init; }

        [JsonPropertyName("pending_change")]
        public double PendingChange { get; init; }

        [JsonConstructor]
        public Wallet()
        {
        }
    }
}
