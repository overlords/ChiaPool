using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class MinerUpdateResult
    {
        [JsonPropertyName("s")]
        public bool Successful { get; init; }
        [JsonPropertyName("r")]
        public string Reason { get; init; }

        [JsonConstructor]
        public MinerUpdateResult()
        {
        }
        private MinerUpdateResult(bool successful, string reason)
        {
            Successful = successful;
            Reason = reason;
        }

        public static MinerUpdateResult FromSuccess()
            => new MinerUpdateResult(true, default);
        public static MinerUpdateResult FromFailed(string reason)
            => new MinerUpdateResult(false, reason);
    }
}
