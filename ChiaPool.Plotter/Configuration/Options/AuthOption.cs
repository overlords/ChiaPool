using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class AuthOption : Option
    {
        public string Token { get; set; } = Environment.GetEnvironmentVariable("token");

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (!Guid.TryParse(Token, out _))
            {
                return ValueTask.FromResult(ValidationResult.Failed("Miner token has invalid format!"));
            }

            return ValueTask.FromResult(ValidationResult.Success);
        }
    }
}
