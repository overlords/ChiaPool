namespace ChiaPool.Models
{
    public sealed class PoolInfo
    {
        public string Name { get; set; }

        public int TotalMinerCount { get; set; }
        public int ActiveMinerCount { get; set; }

        public int MinerPlots { get; set; }

        public int TotalPlotterCount { get; set; }
        public int ActivePlotterCount { get; set; }

        public int PlotterCapactity { get; set; }

        public int PlotterPlots { get; set; }

        public PoolInfo()
        {
        }
    }
}
