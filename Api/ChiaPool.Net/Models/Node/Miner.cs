﻿using System;
using System.Text.Json.Serialization;

namespace ChiaPool.Models
{
    public sealed class Miner : INode
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Token { get; set; }
        public long PlotMinutes { get; set; }

        public short LastPlotCount { get; set; }


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
            LastPlotCount = 0;
            OwnerId = ownerId;
        }
    }
}