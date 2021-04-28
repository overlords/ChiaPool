using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class PlotInfo
    {
        [JsonPropertyName("p")]
        public string PublicKey { get; init; }

        [JsonConstructor]
        public PlotInfo()
        {
        }
        public PlotInfo(string publicKey)
        {
            PublicKey = publicKey;
        }
    }
}
