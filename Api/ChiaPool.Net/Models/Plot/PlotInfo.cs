using System;
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

        public override bool Equals(object obj) 
            => obj is PlotInfo plotInfo && plotInfo.PublicKey == PublicKey;

        public override int GetHashCode() 
            => HashCode.Combine(PublicKey);
    }
}
