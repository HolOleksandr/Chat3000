using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Blazor.Server.Helpers.Interfaces
{
    public interface IHubConnectionService
    {
        Task<HubConnection> ConnectToHub();
    }
}
