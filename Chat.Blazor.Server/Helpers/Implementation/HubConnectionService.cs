using Chat.Blazor.Server.Helpers.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using ServiceStack.Model;
using System.Collections.Concurrent;

namespace Chat.Blazor.Server.Helpers.Implementation
{
    public class HubConnectionService : IHubConnectionService, IDisposable
    {
        private readonly AuthenticationStateProvider _authStateProvieder;
        private readonly NavigationManager _navigationManager;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly ConcurrentDictionary<string, HubConnection> _hubs = new();

        public HubConnectionService(AuthenticationStateProvider authenticationStateProvider, IConfiguration configuration, NavigationManager navigationManager)
        {
            _authStateProvieder = authenticationStateProvider;
            _configuration = configuration;
            _baseUrl = _configuration["ChatApi"];
            _navigationManager = navigationManager;
        }

        public async Task<HubConnection> ConnectToChatHub(string chatId)
        {
            var hubUrl = $"{_baseUrl}chathub?chatId={chatId}";
            return await GetHubOrBuildNewAsync(hubUrl);
        }

        public async Task<HubConnection> ConnectToVideoCallHub(string userId)
        {
            var hubUrl = $"{_baseUrl}videocallhub?userId={userId}";
            return await GetHubOrBuildNewAsync(hubUrl);
        }

        private async Task<HubConnection> GetHubOrBuildNewAsync(string hubUrl)
        {
            var result = _hubs.TryGetValue(hubUrl, out var hubConnection);
            if (!result)
            {
                hubConnection = await BuildHubConnection(hubUrl);
                _hubs.TryAdd(hubUrl, hubConnection);
            }

            return hubConnection;
        }

        private async Task<HubConnection> BuildHubConnection(string hubConnectionUri)
        {
            var token = await GetTokenAsync();
            var hubConnection = new HubConnectionBuilder().WithUrl(_navigationManager.ToAbsoluteUri(hubConnectionUri), options =>
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
            catch (HttpRequestException) { }
            return hubConnection;
        }

        private async Task<string> GetTokenAsync()
        {
            return await ((CustomAuthStateProvider)_authStateProvieder).GetTokenAsync();
        }

        public async Task DisconnectFromHubsAsync()
        {
            foreach (var hub in _hubs)
            {
                await hub.Value.DisposeAsync();
            }
            _hubs.Clear();
        }

        public void Dispose()
        {
            var result = DisconnectFromHubsAsync();
        }
    }
}
