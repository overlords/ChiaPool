using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class PlotterActivationResult
    {
        [JsonPropertyName("s")]
        public bool Successful { get; init; }
        [JsonPropertyName("u")]
        public long UserId { get; init; }
        [JsonPropertyName("r")]
        public string Reason { get; init; }

        [JsonConstructor]
        public PlotterActivationResult()
        {
        }
        private PlotterActivationResult(bool successful, long userId, string reason)
        {
            Successful = successful;
            UserId = userId;
            Reason = reason;
        }

        public static PlotterActivationResult FromSuccess(long userId)
            => new PlotterActivationResult(true, userId, default);

        public static PlotterActivationResult FromAlreadyActive()
            => FromFailed("There already is a active connection from this plotter");

        public static PlotterActivationResult FromError()
            => FromFailed("An unknown error occurred!");

        private static PlotterActivationResult FromFailed(string reason)
            => new PlotterActivationResult(false, default, reason);
    }
}
