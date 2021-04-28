using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class MinerActivationResult
    {
        [JsonPropertyName("s")]
        public bool Successful { get; init; }
        [JsonPropertyName("u")]
        public long UserId { get; init; }
        [JsonPropertyName("r")]
        public string Reason { get; init; }

        [JsonConstructor]
        public MinerActivationResult()
        {
        }
        private MinerActivationResult(bool successful, long userId, string reason)
        {
            Successful = successful;
            UserId = userId;
            Reason = reason;
        }

        public static MinerActivationResult FromSuccess(long userId)
            => new MinerActivationResult(true, userId, default);
        public static MinerActivationResult FromFailed(string reason)
            => new MinerActivationResult(false, default, reason);
    }
}
