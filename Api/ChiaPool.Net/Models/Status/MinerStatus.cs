using System;

namespace ChiaPool.Models
{
    public class MinerStatus : IStatus
    {
        public int PlotCount { get; init; }


        public MinerStatus(int plotCount)
        {
            PlotCount = plotCount;
        }
        public MinerStatus()
        {
        }

        public override bool Equals(object obj)
            => obj is MinerStatus status &&
               PlotCount == status.PlotCount;

        public override int GetHashCode()
            => HashCode.Combine(PlotCount);
    }
}
