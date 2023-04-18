﻿using Blazored.LocalStorage;
using Chat.Blazor.Server.Helpers.Interfaces;
using System.Net.Http.Headers;

namespace Chat.Blazor.Server.Helpers.Realization
{
    public class CustomHttpClient : ICustomHttpClient
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomHttpClient(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetWithTokenAsync(string url)
        {
            var token = await GetTokenASync();
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await httpClient.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> PostWithTokenAsync(string url, HttpContent content)
        {
            var token = await GetTokenASync();
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var response = await httpClient.PostAsync(url, content);
            return response;
        }

        private async Task<string> GetTokenASync()
        {
            try
            {
                var savedToken = await _localStorage.GetItemAsync<string>("authToken");
                return savedToken;
            }
            catch (InvalidOperationException)
            {
                return string.Empty;
            }
            
        }
    }
}