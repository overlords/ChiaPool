using ChiaPool.Models;
using Common.Services;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public class StatusService : Service
    {
        public async Task<MinerStatus> GetStatusAsync()
        {
            return new MinerStatus();
        }
    }
}
