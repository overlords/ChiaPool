using System;
using System.Net;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public class Plotter : INode
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Token { get; set; }
        public long PlotMinutes { get; set; }

        public short LastCapacity { get; set; }
        public short LastAvailablePlots { get; set; }


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
            LastCapacity = 0;
            LastAvailablePlots = 0;
            PlotMinutes = 0;
            OwnerId = ownerId;
        }
    }
}
