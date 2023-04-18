using Blazored.LocalStorage;
using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using static System.Net.WebRequestMethods;
using Chat.Blazor.Server.Helpers.Realization;

namespace Chat.Blazor.Server.Services.Realization
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly string _baseUrl = "https://localhost:7121/";

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegistrationResult> Register(UserRegistrationModel registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync(_baseUrl + "api/account/register", registerModel);
            if (result.IsSuccessStatusCode)
                return new RegistrationResult { Success = true, Errors = null };
            //var registrationResult = JsonSerializer.Deserialize<RegistrationResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new RegistrationResult { Success = false, Errors = new List<string> { "Error occured" } };
        }

        public async Task<LoginResult> Login(UserLoginModel loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync(_baseUrl + "api/account/login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                return loginResult!;
            }

            await _localStorage.SetItemAsync("authToken", loginResult!.Token);
            ((CustomAuthStateProvider)_authenticationStateProvider).NotifyAuthState();
            ((CustomAuthStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task<RegistrationResult> ChangePassword(ChangePasswordModel changePassModel)
        {
            var result = await _httpClient.PostAsJsonAsync(_baseUrl + "api/account/changepass", changePassModel);
            if (result.IsSuccessStatusCode)
            {
                await Logout();
                return new RegistrationResult { Success = true, Errors = null };
            }
            //var registrationResult = JsonSerializer.Deserialize<RegistrationResult>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new RegistrationResult { Success = false, Errors = new List<string> { "Error occured" } };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
