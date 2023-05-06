using Chat.Blazor.Server.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using ServiceStack.Blazor;

namespace Chat.Blazor.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly Dictionary<string, string> Users = new Dictionary<string, string>();

        public void SubscribeForMessages(string userEmail)
        {
            Users.Add(userEmail, Context.ConnectionId);
        }


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




        // Call HUB

        public async Task SendCallMessage(string senderEmail, string receiverEmail, string message)
        {
            await Clients.Group(receiverEmail).SendAsync("ReceiveCallMessage", senderEmail, receiverEmail, message);
        }

        public async Task HangUp(string userId)
        {
            await Clients.Group(userId).SendAsync("CallFinished");
            // TODO leave group
        }
    }
}

