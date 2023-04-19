using Blazored.LocalStorage;
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
            using var client = await GetClientWithTokenAsync();
            var response = await client.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> PostWithTokenAsync(string url, HttpContent content)
        {
            using var client = await GetClientWithTokenAsync();
            var response = await client.PostAsync(url, content);
            return response;
        }

       


        private async Task<HttpClient> GetClientWithTokenAsync()
        {
            var token = await GetTokenAsync();
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return httpClient;
        }
            

        private async Task<string> GetTokenAsync()
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


        // TEST Method
        public async Task<HttpResponseMessage> SendWithTokenAsync(HttpMethod method, string url, HttpContent content = null)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(method, url) { Content = content };
            var response = await httpClient.SendAsync(request);
            return response;
        }
        // TEST Method
    }


}
