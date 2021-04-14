using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class ServerOption : Option
    {
        public Uri PoolHost { get; set; }

        private string PoolHostRaw { get; set; } = Environment.GetEnvironmentVariable("pool_host");

        protected override async ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (string.IsNullOrWhiteSpace(PoolHostRaw))
            {
                return ValidationResult.Failed("Could not find \"pool_host\" environment variable!");
            }

            if (!Uri.TryCreate(PoolHostRaw, UriKind.Absolute, out var ph))
            {
                return ValidationResult.Failed($"Could not parse pool host \"{PoolHostRaw}\" to URL");
            }

            PoolHost = ph;

            var result = await base.ValidateAsync(provider);
            return result;
        }
    }
}
