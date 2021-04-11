namespace ChiaPool.Models
{
    public class RemotePlot
    {
        public long PlotId { get; init; }
        public string DownloadAddress { get; init; }

        public RemotePlot(long plotId, string downloadAddress)
        {
            PlotId = plotId;
            DownloadAddress = downloadAddress;
        }
    }
}
