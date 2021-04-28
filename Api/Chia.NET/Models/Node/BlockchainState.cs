using Chia.NET.Parser;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class BlockchainState
    {
        [JsonPropertyName("difficulty")]
        public int Difficulty { get; init; }

        [JsonPropertyName("genesis_challenge_initialized")]
        public bool GenesisChallengeInitiated { get; init; }

        [JsonPropertyName("peak")]
        public BlockRecord Peak { get; init; }

        [JsonPropertyName("space")]
        [JsonConverter(typeof(JsonBigIntegerConverter))]
        public BigInteger Space { get; init; }

        [JsonPropertyName("sync")]
        public SyncState SyncState { get; init; }

        [JsonConstructor]
        public BlockchainState()
        {
        }
    }
}
