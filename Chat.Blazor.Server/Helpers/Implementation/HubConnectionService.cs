using Chat.Blazor.Server.Helpers.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Blazor.Server.Helpers.Implementation
{
    public class HubConnectionService : IHubConnectionService
    {
        private readonly AuthenticationStateProvider _authStateProvieder;
        private readonly NavigationManager _navigationManager;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public HubConnectionService(AuthenticationStateProvider authenticationStateProvider, IConfiguration configuration, NavigationManager navigationManager)
        {
            _authStateProvieder = authenticationStateProvider;
            _configuration = configuration;
            _baseUrl = _configuration["ApiUrls:ChatApi"];
            _navigationManager = navigationManager;
        }
        public async Task<HubConnection> ConnectToHub()
        {
            var token = await ((CustomAuthStateProvider)_authStateProvieder).GetTokenAsync();
            var hubConnection = new HubConnectionBuilder().WithUrl(_navigationManager.ToAbsoluteUri($"{_baseUrl}chatHub"), options =>
            {
                options.Headers.Add("Authorization", "Bearer " + token);
            }).Build();

            try
            {
                if (token != null)
                {
                    await hubConnection.StartAsync();
                }
            }
            catch (HttpRequestException){}
            return hubConnection;
        }
    }
}
