using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs
{
    public class VideoCallsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()!.Request.Query["userId"];
            if (string.IsNullOrEmpty(userId))
                return;
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.GetHttpContext()!.Request.Query["userId"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task Call(string callingUserId, string callingUserName, string userId, string peerId)
        {
            await Clients.Group(userId).SendAsync("IncomingCall", callingUserId, callingUserName, peerId);
        }

        public async Task HangUp(string userId)
        {
            await Clients.Group(userId).SendAsync("CallTerminated");
        }

        public async Task DeclineCall(string userId)
        {
            await Clients.Group(userId).SendAsync("CallDeclined");
        }

        public async Task UploadPdfFile(string uploaderId, string uploaderName, string receiverId, string fileId)
        {
            await Clients.Group(receiverId).SendAsync("NewPdfContract", uploaderId, uploaderName, fileId);
        }

        public async Task SignPdfFile(string fileName, string senderName, string receiverId, string fileId)
        {
            await Clients.Group(receiverId).SendAsync("ContractIsSigned", fileName, senderName, fileId);
        }
    }
}
