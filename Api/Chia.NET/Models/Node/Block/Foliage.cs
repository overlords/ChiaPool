using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class Foliage
    {
        [JsonPropertyName("foliage_block_data_signature")]
        public string BlockDataSignature { get; init; }
        [JsonPropertyName("foliage_block_data")]
        public BlockData BlockData { get; init; }
        [JsonPropertyName("reward_block_hash")]
        public string RewardBlockHash { get; init; }

        [JsonConstructor]
        public Foliage()
        {
        }
    }
}
