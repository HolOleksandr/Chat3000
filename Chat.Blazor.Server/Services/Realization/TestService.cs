using Blazored.LocalStorage;
using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Chat.Blazor.Server.Services.Realization
{
    public class TestService : ITestService
    {
        private readonly string _baseUrl = "";
        private readonly IConfiguration _configuration;
        private readonly ICustomHttpClient _customHttpClient;


        public TestService( ICustomHttpClient customHttpClient, IConfiguration configuration)
        {
            _customHttpClient = customHttpClient;
            _configuration = configuration;
            _baseUrl = _configuration["ApiUrls:ChatApi"];
        }

        public async Task<string> GetTestMessage()
        {
            //var _baseUrl = _configuration["ApiUrls:ChatApi"];
            var result = await _customHttpClient.GetWithTokenAsync(_baseUrl + "api/test/");

            if (result.IsSuccessStatusCode)
            {
                var message =  await result.Content.ReadAsStringAsync();
                return "Welcome: " + message; 
            }
            return "You are Unauthorized";
        }

    }
}
