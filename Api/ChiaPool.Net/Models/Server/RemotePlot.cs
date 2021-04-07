namespace ChiaPool.Models
{
    public class RemotePlot
    {
        public string DownloadAddress { get; init; }

        public RemotePlot(string downloadAddress)
        {
            DownloadAddress = downloadAddress;
        }
    }
}
