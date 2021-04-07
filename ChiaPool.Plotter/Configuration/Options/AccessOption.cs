using Common.Configuration;
using System;

namespace ChiaPool.Configuration
{
    public class AccessOption : Option
    {
        public string PublicAddress { get; set; } = Environment.GetEnvironmentVariable("public_address");
    }
}
