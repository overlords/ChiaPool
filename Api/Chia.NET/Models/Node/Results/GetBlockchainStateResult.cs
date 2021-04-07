using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    internal sealed class GetBlockchainStateResult : ChiaResult
    {
        [JsonPropertyName("blockchain_state")]
        public BlockchainState BlockchainState { get; init; }

        [JsonConstructor]
        public GetBlockchainStateResult()
        {
        }
    }
}
