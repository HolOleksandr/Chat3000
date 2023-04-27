using Chat.Blazor.Server.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Blazor.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageAsync(MessageDTO message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task ChatNotificationAsync(string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveChatNotification");
        }
    }
}

