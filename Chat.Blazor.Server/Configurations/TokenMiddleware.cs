using Blazored.LocalStorage;

namespace Chat.Blazor.Server.Configurations
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILocalStorageService _localStorage;

        public TokenMiddleware(RequestDelegate next, ILocalStorageService localStorageService)
        {
            _next = next;
            _localStorage = localStorageService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                await _next(httpContext);
                return;
            }

            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(savedToken))
            {
                await _next(httpContext);
                return;
            }

            httpContext.Request.Headers.Add("Authorization", $"Bearer {savedToken}");
            await _next(httpContext);
        }
    }
}
