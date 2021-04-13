using System;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public class Plotter : INode
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Token { get; set; }
        public long Earnings { get; set; }


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
            OwnerId = ownerId;
        }
    }
}
