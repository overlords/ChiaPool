using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class DatabaseOptions : Option
    {
        public string ConnectionString = Environment.GetEnvironmentVariable("db_connection");

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            return base.ValidateAsync(provider);
        }
    }
}
