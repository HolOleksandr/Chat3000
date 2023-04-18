using Blazored.LocalStorage;
using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Chat.Blazor.Server.Configurations
{
    public class TokenMiddleware : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthService _authService;

        public TokenMiddleware(ILocalStorageService localStorage, IAuthService authService)
        {
            //_next = next;
            _localStorage = localStorage;
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var savedToken = await _authService.GetTokenAsync();

            if (request.Headers.Contains("Authorization") || savedToken is null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            request.Headers.Add("Authorization", $"Bearer {savedToken}");


            return await base.SendAsync(request, cancellationToken);
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

        //public async Task InvokeAsync(HttpContext httpContext)
        //{
        //    if (httpContext.Request.Headers.ContainsKey("Authorization"))
        //    {
        //        await _next(httpContext);
        //        return;
        //    }

        //    var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        //    if (string.IsNullOrEmpty(savedToken))
        //    {
        //        await _next(httpContext);
        //        return;
        //    }

        //    httpContext.Request.Headers.Add("Authorization", $"Bearer {savedToken}");
        //    await _next(httpContext);
        //}
    }
}
