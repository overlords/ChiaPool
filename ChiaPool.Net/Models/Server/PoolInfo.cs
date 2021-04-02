namespace ChiaPool.Models.Server
{
    public sealed class PoolInfo
    {
        public string Name { get; set; }

        public int TotalMinerCount { get; set; }
        public int ActiveMinerCount { get; set; }

        public int TotalPlotCount { get; set; }
        public int ActivePlotCount { get; set; }

        public PoolInfo()
        {
        }
    }
}
