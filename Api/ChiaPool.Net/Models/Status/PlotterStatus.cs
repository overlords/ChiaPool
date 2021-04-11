using System;

namespace ChiaPool.Models
{
    public class PlotterStatus : IStatus
    {
        public int Capacity { get; init; }
        public int PlotsAvailable { get; init; }

        public PlotterStatus()
        {
        }

        public override bool Equals(object obj)
            => obj is PlotterStatus status &&
               status.Capacity == Capacity &&
               status.PlotsAvailable == PlotsAvailable;

        public override int GetHashCode()
            => HashCode.Combine(Capacity, PlotsAvailable);
    }
}
