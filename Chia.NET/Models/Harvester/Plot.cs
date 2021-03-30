using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class Plot
    {
        [JsonPropertyName("file_size")]
        public int FileSize { get; }

        [JsonPropertyName("filename")]
        public string FileName { get; }

        [JsonPropertyName("plot_public_key")]
        public string PublicKey { get; }

        [JsonConstructor]
        internal Plot()
        {
        }
    }
}
