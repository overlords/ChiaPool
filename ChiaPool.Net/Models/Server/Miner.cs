using System;
using System.Net;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class Miner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Token { get; set; }
        public IPAddress LastAddress { get; set; }

        public short LastPlotCount { get; set; }
        public long PlotMinutes { get; set; }
        public DateTimeOffset NextIncrement { get; set; }

        [JsonIgnore]
        public User Owner { get; set; }
        public long OwnerId { get; set; }

        public Miner()
        {
        }
        public Miner(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Token = Guid.NewGuid().ToString();
            PlotMinutes = 0;
            NextIncrement = DateTimeOffset.UtcNow;
            LastPlotCount = 0;
        }
    }
}
