using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    internal sealed class GetBlockResult : ChiaResult
    {
        [JsonPropertyName("block")]
        public Block Block { get; init; }

        [JsonConstructor]
        public GetBlockResult()
        {
        }
    }
}
