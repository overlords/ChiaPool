using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration
{
    public class AuthOption : Option
    {
        public string Token { get; init; } = Environment.GetEnvironmentVariable("token");
        public string Name { get; init; } = Environment.GetEnvironmentVariable("username");
        public string Password { get; init; } = Environment.GetEnvironmentVariable("password");

        protected override ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (!Guid.TryParse(Token, out _))
            {
                return ValueTask.FromResult(ValidationResult.Failed("Miner token has invalid format!"));
            }

            return Name == default || Password == default
                ? ValueTask.FromResult(ValidationResult.Failed("Please provide a username and password"))
                : ValueTask.FromResult(ValidationResult.Success);
        }
    }
}
