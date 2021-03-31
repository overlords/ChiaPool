using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaMiningManager.Configuration.Options
{
    public class ServerOptions : Option
    {
        public string PoolHost { get; set; } = Environment.GetEnvironmentVariable("pool_host");
        private string ManagerPortString { get; set; } = Environment.GetEnvironmentVariable("manager_port");
        public short ManagerPort { get; set; }

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
