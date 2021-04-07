using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class ServerOption : Option
    {
        public string PoolHost { get; init; } = Environment.GetEnvironmentVariable("pool_host");
        private string ManagerPortString { get; init; } = Environment.GetEnvironmentVariable("manager_port");
        public short ManagerPort { get; private set; }

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (!short.TryParse(ManagerPortString, out short port))
            {
                return ValueTask.FromResult(ValidationResult.Failed("Port is not a valid number"));
            }

            ManagerPort = port;
            return ValueTask.FromResult(ValidationResult.Success);
        }
    }
}
