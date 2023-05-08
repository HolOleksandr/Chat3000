using Blazored.LocalStorage;
using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Services.Interfaces;
using Chat.Blazor.Server.Services.Implementation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using System.Net.Http.Headers;

namespace Chat.Blazor.Server.Helpers.Implementation
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
            using var client = await GetClientWithTokenAsync();
            var response = await client.GetAsync(url);
            await InterceptResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> PostWithTokenAsync<T>(string url, T content) where T : class
        {
            using var client = await GetClientWithTokenAsync();
            var response = await client.PostAsJsonAsync(url, content);
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
                    case System.Net.HttpStatusCode.Unauthorized:
                        _navigationManager.NavigateTo("/login");
                        await _authService.Logout();
                        await _authStateProvider.GetAuthenticationStateAsync();
                        break;
                    case System.Net.HttpStatusCode.Forbidden:
                        _navigationManager.NavigateTo("/403");
                        break;
                    default:
                        _navigationManager.NavigateTo("/500");
                        break;
                }
            }
        }
    }
}



