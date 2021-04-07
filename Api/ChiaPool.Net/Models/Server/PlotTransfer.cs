using System;

namespace ChiaPool.Models
{
    public class PlotTransfer
    {
        public Guid Id { get; init; }
        public long PlotterId { get; init; }
        public long MinerId { get; init; }

        public long Cost { get; init; }

        public string DownloadAddress { get; init; }
        public DateTimeOffset Deadline { get; init; }

        public PlotTransfer()
        {
        }
        public PlotTransfer(long plotterId, long minerId, long cost, string downloadAddress, int deadlineHours)
        {
            Id = Guid.NewGuid();
            PlotterId = plotterId;
            MinerId = minerId;
            Cost = cost;

            DownloadAddress = downloadAddress;
            Deadline = DateTimeOffset.UtcNow + TimeSpan.FromHours(deadlineHours);
        }
    }
}
