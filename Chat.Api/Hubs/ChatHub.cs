using Chat.BLL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs
{
    [Authorize(Policy = "AllUsers")]
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var chatId = Context.GetHttpContext()!.Request.Query["chatId"];
            if (string.IsNullOrEmpty(chatId))
                return;
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var chatId = Context.GetHttpContext()!.Request.Query["chatId"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToGroupAsync(MessageDTO message, string groupId)
        {
            await Clients.OthersInGroup(groupId).SendAsync("ReceiveMessage", message);
        }
    }
}

