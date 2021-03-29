using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaMiningManager.Configuration
{
    public class DatabaseOptions : Option
    {
        public string ConnectionString = "";

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            ConnectionString = Environment.GetEnvironmentVariable("db_connection");

            return base.ValidateAsync(provider);
        }
    }
}
