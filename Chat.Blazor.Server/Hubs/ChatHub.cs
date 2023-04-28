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

        public async Task SendMessageToGroupAsync(MessageDTO message, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        //public async Task ChatNotificationAsync(string groupName)
        //{
        //    await Clients.Group(groupName).SendAsync("ReceiveChatNotification");
        //}

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}

