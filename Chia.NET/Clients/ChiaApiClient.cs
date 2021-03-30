using Chia.NET.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Chia.NET.Clients
{
    public abstract class ChiaApiClient
    {
        private readonly HttpClient Client;

        public ChiaApiClient(string certName)
        {
            string certificatePath = Path.Combine("/root/.chia/mainnet/config/ssl", certName, $"private_{certName}.crt");
            string keyPath = Path.Combine(certName, $"private_{certName}.key");

            var cert = X509Certificate2.CreateFromPemFile(certificatePath, keyPath);
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
            Client = new HttpClient(handler);
        }

        protected async Task<T> PostAsync<T>(Uri requestUri, IDictionary<string, string> parameters = null) where T : ChiaResult
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new FormUrlEncodedContent(parameters ?? new Dictionary<string, string>())
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<T>();

            return !result.Success 
                ? throw new Exception("Chia responded with unsuccessful") 
                : result;
        }

        protected Task PostAsync(Uri requestUri, IDictionary<string, string> parameters = null) 
            => PostAsync<ChiaResult>(requestUri, parameters);
    }
}
