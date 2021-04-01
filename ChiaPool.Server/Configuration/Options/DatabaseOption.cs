using Common.Configuration;
using System;

namespace ChiaPool.Configuration
{
    public class DatabaseOption : Option
    {
        public string ConnectionString { get; init; } = Environment.GetEnvironmentVariable("db_connection");
    }
}
