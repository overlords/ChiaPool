using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public sealed class ProofOfSpace
    {
        [JsonPropertyName("challenge")]
        public string Challenge { get; init; }
        [JsonPropertyName("plot_public_key")]
        public string PlotPublicKey { get; init; }
        [JsonPropertyName("pool_public_key")]
        public string PoolPublicKey { get; init; }
        [JsonPropertyName("proof")]
        public string Proof { get; init; }
        [JsonPropertyName("size")]
        public int Size { get; init; }

        [JsonConstructor]
        public ProofOfSpace()
        {
        }
    }
}
