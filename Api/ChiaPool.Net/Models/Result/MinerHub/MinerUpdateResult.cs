using System;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class MinerUpdateResult
    {
        [JsonPropertyName("s")]
        public bool Successful { get; init; }
        [JsonPropertyName("c")]
        public PlotInfo[] Conflicts { get; init; }
        [JsonPropertyName("r")]
        public string Reason { get; init; }

        [JsonConstructor]
        public MinerUpdateResult()
        {
        }
        private MinerUpdateResult(bool successful, PlotInfo[] conflicts, string reason)
        {
            Successful = successful;
            Conflicts = conflicts ?? Array.Empty<PlotInfo>();
            Reason = reason;
        }

        public static MinerUpdateResult FromSuccess()
            => new MinerUpdateResult(true, default, default);

        public static MinerUpdateResult FromConflicingPlots(PlotInfo[] conflicts)
            => new MinerUpdateResult(true, conflicts, default);

        public static MinerUpdateResult FromInvalidConnection()
            => new MinerUpdateResult(false, default, "You cannot update this miner from this connection");

        public static MinerUpdateResult FromError()
            => FromFailed("An unknown error occurred");

        private static MinerUpdateResult FromFailed(string reason)
            => new MinerUpdateResult(false, default, reason);
    }
}
