using Common.Configuration;

namespace ChiaMiningManager.Configuration
{
    public class DatabaseOptions : Option
    {
        public string Host { get; set; } = "DbDomain.com";
        public string Database { get; set; } = "AccountAPI";
        public string Username { get; set; } = "AddUserName";
        public string Password { get; set; } = "AddPassword";

        public string BuildConnectionString()
            => $"Host={Host};Database={Database};Username={Username};Password={Password}";
    }
}
