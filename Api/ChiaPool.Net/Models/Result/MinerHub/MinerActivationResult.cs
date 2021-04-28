using System;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class MinerActivationResult
    {
        [JsonPropertyName("s")]
        public bool Successful { get; init; }
        [JsonPropertyName("u")]
        public long UserId { get; init; }
        [JsonPropertyName("c")]
        public PlotInfo[] Conflicts { get; init; }
        [JsonPropertyName("r")]
        public string Reason { get; init; }

        [JsonConstructor]
        public MinerActivationResult()
        {
        }
        private MinerActivationResult(bool successful, long userId, PlotInfo[] conflicts, string reason)
        {
            Successful = successful;
            UserId = userId;
            Conflicts = conflicts ?? Array.Empty<PlotInfo>();
            Reason = reason;
        }

        public static MinerActivationResult FromSuccess(long userId)
            => new MinerActivationResult(true, userId, default, default);

        public static MinerActivationResult FromConflicingPlots(long userId, PlotInfo[] conflicts)
            => new MinerActivationResult(true, userId, conflicts, default);

        public static MinerActivationResult FromDuplicates()
            => FromFailed("Cannot activate duplicate plot public keys");
        public static MinerActivationResult FromStatusPlotCountUnmatch()
            => FromFailed("Status PlotCount and PlotInfos count unequal");
        public static MinerActivationResult FromAlreadyActive()
            => FromFailed("There already is a active connection from this miner");

        public static MinerActivationResult FromError()
            => FromFailed("An unknown error occurred");

        private static MinerActivationResult FromFailed(string reason)
            => new MinerActivationResult(false, default, default, reason);
    }
}
