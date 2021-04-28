using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class PlotterUpdateResult
    {
        [JsonPropertyName("s")]
        public bool Successful { get; init; }
        [JsonPropertyName("r")]
        public string Reason { get; init; }

        [JsonConstructor]
        public PlotterUpdateResult()
        {
        }
        private PlotterUpdateResult(bool successful, string reason)
        {
            Successful = successful;
            Reason = reason;
        }

        public static PlotterUpdateResult FromSuccess()
            => new PlotterUpdateResult(true, default);
        public static PlotterUpdateResult FromFailed(string reason)
            => new PlotterUpdateResult(false, reason);
    }
}
