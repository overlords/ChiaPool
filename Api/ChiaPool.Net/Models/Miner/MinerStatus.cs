namespace ChiaPool.Models
{
    public class MinerStatus
    {
        public int PlotCount { get; init; }


        public MinerStatus(int plotCount)
        {
            PlotCount = plotCount;
        }
        public MinerStatus()
        {
        }
    }
}
