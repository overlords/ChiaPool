using System.Net;

namespace ChiaPool.Models
{
    public sealed class MinerActivation
    {
        public string ConnectionId { get; set; }
        public IPAddress Address { get; set; }
        public MinerStatus Status { get; set; }

        public MinerActivation(string connectionId, IPAddress address, MinerStatus status)
        {
            ConnectionId = connectionId;
            Address = address;
            Status = status;
        }
    }
}
