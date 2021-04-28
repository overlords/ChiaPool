using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    internal sealed class GetBlocksResult : ChiaResult
    {
        [JsonPropertyName("blocks")]
        public Block[] Blocks { get; init; }

        [JsonConstructor]
        public GetBlocksResult()
        {
        }
    }
}
