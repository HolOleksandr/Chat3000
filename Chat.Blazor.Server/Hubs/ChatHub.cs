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
        //TODO: users must receive notification if they have new messages in their groups. 




        // Call HUB

        public async Task SendCallMessage(string senderEmail, string receiverEmail, string message)
        {
            await Clients.All.SendAsync("ReceiveCallMessage", senderEmail, receiverEmail, message);
            //Users.TryGetValue(receiverEmail, out var connectionId);
            //await Clients.User(connectionId).SendAsync("ReceiveCallMessage", senderEmail, receiverEmail, message);
        }
    }
}

