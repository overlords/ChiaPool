using System.Collections.Generic;
using System.Linq;

namespace ChiaPool.Models
{
    public sealed class MinerState
    {
        public int MinerCount { get; set; }
        public long PlotMinutes { get; set; }
        public int PlotCount { get; set; }

        public MinerState()
        {
        }

        public static MinerState FromMiners(IEnumerable<Miner> miners)
            => new MinerState()
            {
                MinerCount = miners.Count(),
                PlotCount = miners.Sum(x => x.LastPlotCount),
                PlotMinutes = miners.Sum(x => x.PlotMinutes)
            };
    }
}
