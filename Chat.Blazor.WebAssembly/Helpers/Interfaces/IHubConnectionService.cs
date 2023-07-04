using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Blazor.WebAssembly.Helpers.Interfaces
{
    public interface IHubConnectionService
    {
        Task<HubConnection> ConnectToChatHub(string chatId);
        Task<HubConnection> ConnectToVideoCallHub(string userId);
        Task DisconnectFromHubsAsync();
    }
}
