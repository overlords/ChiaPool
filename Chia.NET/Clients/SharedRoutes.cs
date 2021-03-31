using System;

namespace Chia.NET.Clients
{
    internal static class SharedRoutes
    {
        public static Uri GetConnections(string apiUrl)
            => new Uri(apiUrl + "get_connections");
    }
}
