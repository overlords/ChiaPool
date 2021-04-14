using ChiaPool.Models;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public sealed class UserService : Service
    {
        public async Task<long> GetOwnerIdFromMinerId(long minerId)
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();

            return await dbContext.Miners
                .Where(x => x.Id == minerId)
                .Select(x => x.OwnerId)
                .FirstOrDefaultAsync();
        }

        public async Task<long> GetOwnerIdFromPlotterId(long plotterId)
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();

            return await dbContext.Plotters
                .Where(x => x.Id == plotterId)
                .Select(x => x.OwnerId)
                .FirstOrDefaultAsync();
        }
    }
}
