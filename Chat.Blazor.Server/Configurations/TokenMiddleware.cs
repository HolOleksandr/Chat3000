using Blazored.LocalStorage;
using Chat.Blazor.Server.Helpers.Realization;
using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace Chat.Blazor.Server.Configurations
{
    public class TokenMiddleware : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthService _authService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        

        public TokenMiddleware(ILocalStorageService localStorage, IAuthService authService, AuthenticationStateProvider authenticationStateProvider)
        {
            //_next = next;
            _localStorage = localStorage;
            _authService = authService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var savedToken = await _authService.GetTokenAsync();
            var token2 = await GetTokenAsync();

            if (request.Headers.Contains("Authorization") || savedToken is null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            request.Headers.Add("Authorization", $"Bearer {savedToken}");

            var response = await base.SendAsync(request, cancellationToken);
            return response;
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

    }
}
