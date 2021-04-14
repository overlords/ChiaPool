using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public sealed class ServerOption : Option
    {
        public Uri PoolHost { get; set; } 
        public Uri MinerHost { get; set; } 

        private string PoolHostRaw { get; set; } = Environment.GetEnvironmentVariable("chiapoolcli_poolhost");
        private string MinerHostRaw { get; set; } = Environment.GetEnvironmentVariable("chiapoolcli_minerhost");

        public ServerOption()
        {
        }

        protected override async ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (string.IsNullOrWhiteSpace(PoolHostRaw))
            {
                return ValidationResult.Failed("Could not find \"chiapoolcli_poolhost\" environment variable!");
            }
            if (string.IsNullOrWhiteSpace(MinerHostRaw))
            {
                return ValidationResult.Failed("Could not find \"chiapoolcli_minerhost\" environment variable!");
            }

            if (!Uri.TryCreate(PoolHostRaw, UriKind.Absolute, out var ph))
            {
                return ValidationResult.Failed($"Could not parse pool host \"{PoolHostRaw}\" to URL");
            }
            if (!Uri.TryCreate(MinerHostRaw, UriKind.Absolute, out var mh))
            {
                return ValidationResult.Failed($"Could not parse miner host \"{MinerHostRaw}\" to URL");
            }

            PoolHost = ph;
            MinerHost = mh;

            var result = await base.ValidateAsync(provider);
            return result;
        }
    }
}
