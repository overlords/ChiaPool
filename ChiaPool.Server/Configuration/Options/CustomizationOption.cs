using Common.Configuration;
using System;

namespace ChiaPool.Configuration.Options
{
    public class CustomizationOption : Option
    {
        public string PoolName { get; init; } = Environment.GetEnvironmentVariable("pool_name");
    }
}
