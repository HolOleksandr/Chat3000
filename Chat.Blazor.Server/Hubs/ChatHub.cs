using Chat.Blazor.Server.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using ServiceStack.Blazor;

namespace Chat.Blazor.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToGroupAsync(MessageDTO message, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        public async Task ChatNotificationAsync(string groupName)
        {
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveChatNotification");
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        //TODO: users must receive notification if they have new messages in their groups. 




        // Call HUB

        public async Task SendCallMessage(string senderEmail, string receiver, string message)
        {
            await Clients.All.SendAsync("ReceiveCallMessage", senderEmail, receiver, message);
        }
    }
}

