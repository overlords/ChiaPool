using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class BlockchainState
    {
        [JsonPropertyName("difficulty")]
        public int Difficulty { get; init; }

        [JsonPropertyName("genesis_challenge_initialized")]
        public bool GenesisChallengeInitiated { get; init; }

        [JsonPropertyName("space")]
        public long Space { get; init; }

        [JsonPropertyName("sync")]
        public SyncState SyncState { get; init; }

        [JsonConstructor]
        public BlockchainState()
        {
        }
    }
}
