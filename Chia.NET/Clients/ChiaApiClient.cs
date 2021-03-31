using Chia.NET.Models;
using Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Chia.NET.Clients
{
    public abstract class ChiaApiClient : Service
    {
        private const string SslDirectory = "/root/.chia/mainnet/config/ssl";

        private HttpClient Client;
        private readonly string CertName;

        public ChiaApiClient(string certName)
        {
            CertName = certName;
        }

        protected override ValueTask InitializeAsync()
        {
            string certificatePath = Path.Combine(SslDirectory, CertName, $"private_{CertName}.crt");
            string keyPath = Path.Combine(SslDirectory, CertName, $"private_{CertName}.key");
            var certificate = X509Certificate2.CreateFromPemFile(certificatePath, keyPath);

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };
            handler.ClientCertificates.Add(certificate);
            Client = new HttpClient(handler);

            return ValueTask.CompletedTask;
        }

        protected async Task<T> PostAsync<T>(Uri requestUri, IDictionary<string, string> parameters = null) where T : ChiaResult
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = JsonContent.Create(parameters ?? new Dictionary<string, string>())
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
