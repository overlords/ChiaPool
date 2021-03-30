using System.Text.Json.Serialization;

namespace Chia.NET.Models
{
    public class ChiaResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        public ChiaResult()
        {
        }
    }
}
