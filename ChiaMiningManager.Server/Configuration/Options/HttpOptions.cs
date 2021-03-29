using Common.Configuration;
using System.Net;

namespace ChiaMiningManager.Configuration
{
    public class HttpOptions : Option
    {
        public string Address { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 10000;
        public bool Https { get; set; } = false;

        public IPAddress GetAddress()
            => IPAddress.Parse(Address);
    }
}
