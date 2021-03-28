﻿using System;

namespace ChiaMiningManager.Models
{
    public class Miner
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Token { get; set; }

        public short Plots { get; set; }
        public long PlotMinutes { get; set; }
        public DateTimeOffset NextIncrement { get; set; }

        public Miner(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Token = Guid.NewGuid().ToString();
            NextIncrement = DateTimeOffset.UtcNow;
        }
    }
}
