namespace ChiaPool.Models
{
    public class ClientStatus
    {
        public int PlotCount { get; init; }


        public ClientStatus(int plotCount)
        {
            PlotCount = plotCount;
        }
        public ClientStatus()
        {
        }
    }
}
