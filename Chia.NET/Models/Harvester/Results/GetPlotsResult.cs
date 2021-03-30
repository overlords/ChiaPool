using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    internal sealed class GetPlotsResult : ChiaResult
    {
        [JsonPropertyName("failed_to_open_filenames")]
        public string[] FailedToOpenFileNames { get; }

        [JsonPropertyName("not_found_filenames")]
        public string[] NotFoundFilenames { get; }

        [JsonPropertyName("plots")]
        public Plot[] Plots { get; }

        private GetPlotsResult()
        {
        }
    }
}
