using Common.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChiaPool.Api
{
    public abstract class ApiAccessor : Service
    {
        protected readonly HttpClient Client;

        public ApiAccessor(HttpClient client)
        {
            Client = client;
        }

        protected async Task<T> GetAsync<T>(Uri requestUri, string authorization = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            if (authorization != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(authorization);
            }

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode && !ShouldIgnoreStatusCode(response.StatusCode))
            {
                throw new Exception($"There was an error! The server responded with {response.StatusCode}");
            }

            return !response.IsSuccessStatusCode
                ? default
                : await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task GetAsync(Uri requestUri, string authorization = null)
            => await GetAsync<object>(requestUri, authorization);

        protected async Task<T> PostAsync<T>(Uri requestUri, object parameters = null, string authorization = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = JsonContent.Create(parameters ?? new Dictionary<string, string>())
            };
            if (authorization != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(authorization);
            }

            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode && !ShouldIgnoreStatusCode(response.StatusCode))
            {
                throw new Exception($"There was an error! The server responded with {response.StatusCode}");
            }

            return !response.IsSuccessStatusCode
                ? default
                : await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task PostAsync(Uri requestUri, object parameters = null, string authorization = null)
            => await PostAsync<object>(requestUri, parameters, authorization);

        private bool ShouldIgnoreStatusCode(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.NotFound => true,
                HttpStatusCode.Unauthorized => true,
                HttpStatusCode.Conflict => true,
                _ => false,
            };
        }
    }
}
