using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration.Options
{
    public class ServerOption : Option
    {
        public string PoolHost { get; init; } = Environment.GetEnvironmentVariable("pool_host");
        private string RawManagerPort { get; init; } = Environment.GetEnvironmentVariable("manager_port");
        public short ManagerPort { get; private set; }

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (!short.TryParse(RawManagerPort, out short port))
            {
                return ValueTask.FromResult(ValidationResult.Failed("Port is not a valid number"));
            }

            ManagerPort = port;
            return ValueTask.FromResult(ValidationResult.Success);
        }
    }
}
