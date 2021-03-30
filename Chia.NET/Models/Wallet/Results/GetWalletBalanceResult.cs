using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    internal sealed class GetWalletBalanceResult : ChiaResult
    {
        [JsonPropertyName("wallet_balance")]
        public Wallet Wallet { get; }

        [JsonConstructor]
        public GetWalletBalanceResult()
        {
        }
    }
}
