using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class GetWalletBalanceResult : ChiaResult
    {
        [JsonPropertyName("wallet_balance")]
        public Wallet Wallet { get; init; }

        [JsonConstructor]
        public GetWalletBalanceResult()
        {
        }
    }
}
