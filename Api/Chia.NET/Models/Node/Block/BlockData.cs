using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class BlockData
    {
        [JsonPropertyName("farmer_reward_puzzle_hash")]
        public string FarmerRewardPuzzleHash { get; init; }
        [JsonPropertyName("pool_signature")]
        public string PoolSignature { get; init; }
        [JsonPropertyName("pool_target")]
        public PoolTarget PoolTarget { get; init; }

        [JsonConstructor]
        public BlockData()
        {
        }
    }
}
