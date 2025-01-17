﻿using Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
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

            response.EnsureSuccessStatusCode();

            if (typeof(T) == typeof(object))
            {
                return default;
            }
            if (typeof(T) == typeof(string))
            {
                return (T)(object)await response.Content.ReadAsStringAsync();
            }
            if (typeof(T) == typeof(Stream))
            {
                return (T)(object)await response.Content.ReadAsStreamAsync();
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task GetAsync(Uri requestUri, string authScheme = null, string authValue = null)
            => await GetAsync<object>(requestUri, authScheme, authValue);

        protected async Task<T> PostAsync<T>(Uri requestUri, object parameters = null, string authScheme = null, string authValue = null)
        {
            var content = (HttpContent)(parameters is Dictionary<string, string> dict
                ? new FormUrlEncodedContent(dict)
                : JsonContent.Create(parameters));

            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(ApiUrl, requestUri))
            {
                Content = content,
            };
            if (authScheme != default || authValue != default)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(authScheme, authValue);
            }

            var response = await Client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            if (typeof(T) == typeof(object))
            {
                return default;
            }
            if (typeof(T) == typeof(string))
            {
                return (T)(object)await response.Content.ReadAsStringAsync();
            }
            if (typeof(T) == typeof(Stream))
            {
                return (T)(object)await response.Content.ReadAsStreamAsync();
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        protected async Task PostAsync(Uri requestUri, object parameters = null, string authScheme = null, string authValue = null)
            => await PostAsync<object>(requestUri, parameters, authScheme, authValue);

        protected string Base64Encode(string plain)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
    }
}
