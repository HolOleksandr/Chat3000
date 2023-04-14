using Blazored.LocalStorage;
using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Chat.Blazor.Server.Services.Realization
{
    public class TestService : ITestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7121/";
        private readonly ILocalStorageService _localStorage;


        public TestService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorage = localStorageService;
        }

        public async Task<string> GetTestMessage()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            var result = await _httpClient.GetAsync(_baseUrl + "api/test/");
            if (result.IsSuccessStatusCode)
            {
                var message =  await result.Content.ReadAsStringAsync();
                return "Welcome: " + message; 
            }
            return "You are Unauthorized";
        }

    }
}
