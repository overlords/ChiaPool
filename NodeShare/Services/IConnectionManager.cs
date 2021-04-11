using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public interface IConnectionManager
    {
        public Task SendStatusUpdateAsync();
        public Task SendActivateRequestAsync();
    }
}
