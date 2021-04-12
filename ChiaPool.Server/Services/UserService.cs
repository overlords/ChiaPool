using ChiaPool.Models;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaPool.Services
{
    public sealed class UserService : Service
    {
        public Task<long> GetOwnerIdFromMinerId(long minerId)
        {
            using var scope = Provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MinerContext>();

            return dbContext.Miners
                .Where(x => x.Id == minerId)
                .Select(x => x.OwnerId)
                .FirstOrDefaultAsync();
        }
    }
}
