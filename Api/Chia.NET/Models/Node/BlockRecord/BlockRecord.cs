using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class BlockRecord
    {
        [JsonPropertyName("header_hash")]
        public string HeaderHash { get; init; }
        [JsonPropertyName("height")]
        public long Height { get; init; }
        [JsonPropertyName("weight")]
        public long Weight { get; init; }

        [JsonConstructor]
        public BlockRecord()
        {

        }
    }
}
