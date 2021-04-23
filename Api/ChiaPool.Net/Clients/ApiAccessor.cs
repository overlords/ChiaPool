using Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChiaPool.Api
{
    public abstract class ApiAccessor : Service
    {
        private Uri ApiUrl;
        protected readonly HttpClient Client;

        public ApiAccessor(HttpClient client)
        {
            Client = client;
        }

        public void SetApiUrl(Uri apiUrl)
            => ApiUrl = apiUrl;

        protected async Task<T> GetAsync<T>(Uri requestUri, string authScheme = null, string authValue = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(ApiUrl, requestUri));
            if (authScheme != default || authValue != default)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(authScheme, authValue);
            }

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"There was an error! The server responded with {response.StatusCode}");
            }
            if (typeof(T) == typeof(object))
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task GetAsync(Uri requestUri, string authScheme = null, string authValue = null)
            => await GetAsync<object>(requestUri, authScheme, authValue);

        protected async Task<Stream> GetStreamAsync(Uri requestUri, string authScheme = null, string authValue = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(ApiUrl, requestUri));
            if (authScheme != default || authValue != default)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(authScheme, authValue);
            }

            var response = await Client.SendAsync(request);

            return !response.IsSuccessStatusCode
                ? throw new Exception($"There was an error! The server responded with {response.StatusCode}")
                : await response.Content.ReadAsStreamAsync();
        }

        protected async Task<T> PostAsync<T>(Uri requestUri, object parameters = null, string authScheme = null, string authValue = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(ApiUrl, requestUri))
            {
                Content = JsonContent.Create(parameters ?? new Dictionary<string, string>())
            };
            if (authScheme != default || authValue != default)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(authScheme, authValue);
            }

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"There was an error! The server responded with {response.StatusCode}");
            }
            if (typeof(T) == typeof(object))
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task PostAsync(Uri requestUri, object parameters = null, string authScheme = null, string authValue = null)
            => await PostAsync<object>(requestUri, parameters, authScheme, authValue);
    }
}
