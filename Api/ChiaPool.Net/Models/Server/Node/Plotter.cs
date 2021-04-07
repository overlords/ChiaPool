using System;
using System.Net;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public class Plotter : IPoolNode
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Token { get; set; }
        public IPAddress LastAddress { get; set; }

        public short Capacity { get; set; }
        public short AvailablePlots { get; set; }
        public long PlotMinutes { get; set; }
        public DateTimeOffset LastTick { get; set; }

        [JsonIgnore]
        public User Owner { get; set; }
        public long OwnerId { get; set; }

        public Plotter()
        {
        }
        public Plotter(string name, long ownerId)
        {
            Name = name;
            Token = Guid.NewGuid().ToString();
            Capacity = 0;
            AvailablePlots = 0;
            PlotMinutes = 0;
            LastTick = DateTimeOffset.MinValue;
            OwnerId = ownerId;
        }
    }
}
