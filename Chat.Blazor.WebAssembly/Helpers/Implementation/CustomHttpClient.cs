using Chat.Blazor.WebAssembly.Helpers.Interfaces;
using Chat.Blazor.WebAssembly.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using Chat.Blazor.WebAssembly.Exceptions;
using System.Text;
using System.Text.Json;

namespace Chat.Blazor.WebAssembly.Helpers.Implementation
{
    public class CustomHttpClient : ICustomHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authStateProvider;

        public CustomHttpClient(
            IHttpClientFactory httpClientFactory, 
            IAuthService authService,
            NavigationManager navigationManager,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClientFactory = httpClientFactory;
            _authService = authService;
            _navigationManager = navigationManager;
            _authStateProvider = authenticationStateProvider;
        }
        public async Task<HttpResponseMessage> GetWithTokenAsync(string url)
        {
            return await SendRequestWithTokenAsync(url, HttpMethod.Get);
        }

        public async Task<HttpResponseMessage> PostWithTokenAsync<T>(string url, T content) where T : class
        {
            var serializedContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
            return await SendRequestWithTokenAsync(url, HttpMethod.Post, serializedContent);
        }

        public async Task<HttpResponseMessage> DeleteWithTokenAsync(string url)
        {
            return await SendRequestWithTokenAsync(url, HttpMethod.Delete);
        }

        public async Task<HttpResponseMessage> PostContentWithTokenAsync(string url, MultipartFormDataContent content)
        {
            return await SendRequestWithTokenAsync(url, HttpMethod.Post, content);
        }

        private async Task<HttpResponseMessage> SendRequestWithTokenAsync(string url, HttpMethod method, HttpContent? content = null)
        {
            using var client = await GetClientWithTokenAsync();
            HttpResponseMessage response = method.Method.ToUpper() switch
            {
                "GET" => await client.GetAsync(url),
                "POST" => await client.PostAsync(url, content),
                "DELETE" => await client.DeleteAsync(url),
                _ => throw new ArgumentException($"Unsupported HTTP method: {method.Method}"),
            };
            await InterceptResponse(response);
            return response;
        }

        private async Task<HttpClient> GetClientWithTokenAsync()
        {
            var token = await ((CustomAuthStateProvider)_authStateProvider).GetTokenAsync();
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return httpClient;
        }

        private async Task InterceptResponse(HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
            {
                var statusCode = response.StatusCode;

                switch (statusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        _navigationManager.NavigateTo("/login");
                        await _authService.Logout();
                        await _authStateProvider.GetAuthenticationStateAsync();
                        break;
                    case System.Net.HttpStatusCode.Forbidden:
                        _navigationManager.NavigateTo("/403");
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        throw new ChatException("API is not available");
                    default:
                        _navigationManager.NavigateTo("/500");
                        break;
                }
            }
        }
    }
}