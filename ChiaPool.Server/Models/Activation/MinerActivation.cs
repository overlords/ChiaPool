using System.Collections.Generic;

namespace ChiaPool.Models
{
    public sealed class MinerActivation
    {
        public string ConnectionId { get; private set; }
        public MinerStatus Status { get; private set; }
        public List<PlotInfo> PlotInfos { get; private set; }

        public MinerActivation(string connectionId, MinerStatus status, List<PlotInfo> plotInfos)
        {
            ConnectionId = connectionId;
            Status = status;
            PlotInfos = plotInfos;
        }

        public void Update(MinerStatus status, List<PlotInfo> plotInfos)
        {
            Status = status;
            PlotInfos = plotInfos;
        }
    }
}
