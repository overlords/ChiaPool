using System;
using System.Net;

namespace ChiaPool.Models
{
    public sealed class Miner
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Token { get; set; }

        public IPAddress LastAddress { get; set; }
        public short LastPlotCount { get; set; }

        public long PlotMinutes { get; set; }
        public DateTimeOffset NextIncrement { get; set; }

        public Miner()
        {
        }
        public Miner(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Token = Guid.NewGuid().ToString();
            NextIncrement = DateTimeOffset.UtcNow;
            LastPlotCount = 0;
        }
    }
}
