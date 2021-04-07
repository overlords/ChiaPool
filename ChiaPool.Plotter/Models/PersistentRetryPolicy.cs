using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace ChiaPool.Models
{
    public class PersistentRetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            return retryContext.PreviousRetryCount switch
            {
                0 => TimeSpan.FromSeconds(5),
                1 => TimeSpan.FromSeconds(10),
                2 => TimeSpan.FromSeconds(20),
                <= 10 => TimeSpan.FromSeconds(30),
                _ => TimeSpan.FromSeconds(60),
            };

        }
    }
}
