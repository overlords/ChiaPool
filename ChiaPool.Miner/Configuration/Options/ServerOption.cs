using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class ServerOption : Option
    {
        public Uri PoolHost { get; private set; }
        public string FullNodeHost { get; init; } = Environment.GetEnvironmentVariable("node_host");
        public ushort FullNodePort { get; private set; }

        private string FullNodePortRaw { get; init; } = Environment.GetEnvironmentVariable("node_port");
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

            if (string.IsNullOrWhiteSpace(FullNodePortRaw))
            {
                return ValidationResult.Failed("Could not find \"node_port\" environment variable!");
            }
            if (!ushort.TryParse(FullNodePortRaw, out ushort fnp))
            {
                return ValidationResult.Failed($"{FullNodePortRaw} is not a valid port number!");
            }

            FullNodePort = fnp;

            var result = await base.ValidateAsync(provider);
            return result;
        }
    }
}
