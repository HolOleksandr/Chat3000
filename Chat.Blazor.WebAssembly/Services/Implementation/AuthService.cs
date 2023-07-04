using Blazored.LocalStorage;
using Chat.Blazor.WebAssembly.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Chat.Blazor.WebAssembly.Helpers.Implementation;
using System.Net.Http.Json;
using Chat.Blazor.WebAssembly.Models.Requests;
using Chat.Blazor.WebAssembly.Models.Responses;

namespace Chat.Blazor.WebAssembly.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly string _baseUrl = "";
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage,
                            IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _baseUrl = _configuration.GetSection("ChatApi").Value;
        }

        public async Task<RegistrationResult> Register(UserRegistrationModel registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync(_baseUrl + "api/account/register", registerModel);
            if (result.IsSuccessStatusCode)
                return new RegistrationResult { Success = true, Errors = null };

            var stream = await result.Content.ReadAsStreamAsync();
            var registrationResult = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<RegistrationResult>(stream);

            return new RegistrationResult { Success = false, Errors = registrationResult.Errors };
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

            var stream = await result.Content.ReadAsStreamAsync();
            var registrationResult = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<RegistrationResult>(stream);

            return new RegistrationResult { Success = false, Errors = registrationResult.Errors };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}