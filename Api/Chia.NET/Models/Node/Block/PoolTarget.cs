using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class PoolTarget
    {
        [JsonPropertyName("max_height")]
        public long MaxHeight { get; init; }

        [JsonPropertyName("puzzle_hash")]
        public string PuzzleHash { get; init; }

        public PoolTarget()
        {
        }
    }
}
