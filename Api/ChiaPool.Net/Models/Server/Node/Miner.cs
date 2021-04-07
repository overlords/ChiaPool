using System;
using System.Net;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class Miner : IPoolNode
    {
        public long Id { get; set; }
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
        public Miner(string name, long ownerId)
        {
            Name = name;
            Token = Guid.NewGuid().ToString();
            PlotMinutes = 0;
            NextIncrement = DateTimeOffset.UtcNow;
            LastPlotCount = 0;
            OwnerId = ownerId;
        }
    }
}
