using System;

namespace ChiaPool.Models
{
    public class PlotTransfer
    {
        public Guid Id { get; init; }
        public long PlotterId { get; init; }
        public long PlotId { get; init; }
        public long MinerId { get; init; }

        public long Cost { get; init; }

        public string DownloadAddress { get; init; }
        public DateTimeOffset Deadline { get; init; }

        public PlotTransfer()
        {
        }
        public PlotTransfer(long plotterId, long plotId, long minerId, long cost, string downloadAddress, int deadlineHours)
        {
            Id = Guid.NewGuid();
            PlotterId = plotterId;
            PlotId = plotId;
            MinerId = minerId;
            Cost = cost;

            DownloadAddress = downloadAddress;
            Deadline = DateTimeOffset.UtcNow + TimeSpan.FromHours(deadlineHours);
        }
    }
}
