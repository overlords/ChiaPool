using ChiaPool.Models;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public interface IStatusService<T> where T : IStatus 
    {
        public T GetCurrentStatus();
        public Task RefreshStatusAsync();
    }
}
