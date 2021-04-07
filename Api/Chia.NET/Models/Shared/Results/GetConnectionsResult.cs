using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    internal sealed class GetConnectionsResult : ChiaResult
    {
        [JsonPropertyName("connections")]
        public ChiaConnection[] Connections { get; init; }

        [JsonConstructor]
        public GetConnectionsResult()
        {
        }
    }
}
