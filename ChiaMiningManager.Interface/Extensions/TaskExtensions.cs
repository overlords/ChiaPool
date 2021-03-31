using System.Net.Http;
using System.Threading.Tasks;

namespace ChiaMiningManager.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<T> Try<T>(this Task<T> request)
        {
            try
            {
                return await request;
            }
            catch (HttpRequestException)
            {
                return default;
            }
        }
    }
}
