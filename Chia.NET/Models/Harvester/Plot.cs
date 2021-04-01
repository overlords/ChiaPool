using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class Plot
    {
        [JsonPropertyName("file_size")]
        public ulong FileSize { get; init; }

        [JsonPropertyName("filename")]
        public string FileName { get; init; }

        [JsonPropertyName("plot_public_key")]
        public string PublicKey { get; init; }

        [JsonPropertyName("size")]
        public int Size { get; init; }

        [JsonConstructor]
        public Plot()
        {
        }
    }
}
