using Microsoft.AspNetCore.SignalR;

namespace Chat.Blazor.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageAsync(string groupName, string messageText, string userName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", messageText, userName);
        }

        public async Task ChatNotificationAsync(string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveChatNotification");
        }
    }
}
