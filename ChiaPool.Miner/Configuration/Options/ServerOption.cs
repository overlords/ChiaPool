using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class ServerOption : Option
    {
        public Uri PoolHost { get; set; }
        public Uri FarmerHost { get; set; }

        private string PoolHostRaw { get; set; } = Environment.GetEnvironmentVariable("pool_host");
        private string FarmerHostRaw { get; set; } = Environment.GetEnvironmentVariable("farmer_host");

        protected override async ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (string.IsNullOrWhiteSpace(PoolHostRaw))
            {
                return ValidationResult.Failed("Could not find \"pool_host\" environment variable!");
            }
            if (string.IsNullOrWhiteSpace(FarmerHostRaw))
            {
                return ValidationResult.Failed("Could not find \"farmer_host\" environment variable!");
            }

            if (!Uri.TryCreate(PoolHostRaw, UriKind.Absolute, out var ph))
            {
                return ValidationResult.Failed($"Could not parse pool host \"{PoolHostRaw}\" to URL");
            }
            if (!Uri.TryCreate(FarmerHostRaw, UriKind.Absolute, out var fh))
            {
                return ValidationResult.Failed($"Could not parse farmer host \"{FarmerHostRaw}\" to URL");
            }

            PoolHost = ph;
            FarmerHost = fh;

            var result = await base.ValidateAsync(provider);
            return result;
        }
    }
}
