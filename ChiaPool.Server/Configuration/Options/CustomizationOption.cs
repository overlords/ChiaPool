using Common.Configuration;
using System;
using System.Threading.Tasks;

namespace ChiaPool.Configuration.Options
{
    public class CustomizationOption : Option
    {
        public string PoolName { get; init; } = Environment.GetEnvironmentVariable("pool_name");

        public bool AllowUserRegistration { get; private set; }
        public bool AllowMinerCreation { get; private set; }
        public bool AllowPlotterCreation { get; private set; }

        private string AllowUserRegistrationRaw { get; init; } = Environment.GetEnvironmentVariable("allow_user_registration");
        private string AllowMinerCreationRaw { get; init; } = Environment.GetEnvironmentVariable("allow_miner_creation");
        private string AllowPlotterCreationRaw { get; init; } = Environment.GetEnvironmentVariable("allow_plotter_creation");

        protected override async ValueTask<ValidationResult> ValidateAsync(IServiceProvider provider)
        {
            if (!bool.TryParse(AllowUserRegistrationRaw, out bool aur))
            {
                return ValidationResult.Failed("Could not parse value of \"allow_user_registration\" to boolean!");
            }
            if (!bool.TryParse(AllowMinerCreationRaw, out bool amc))
            {
                return ValidationResult.Failed("Could not parse value of \"allow_miner_creation\" to boolean!");
            }
            if (!bool.TryParse(AllowPlotterCreationRaw, out bool apc))
            {
                return ValidationResult.Failed("Could not parse value of \"allow_plotter_creation\" to boolean!");
            }

            AllowUserRegistration = aur;
            AllowMinerCreation = amc;
            AllowPlotterCreation = apc;

            return await base.ValidateAsync(provider);
        }
    }
}
