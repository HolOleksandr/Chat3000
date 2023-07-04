using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Blazor.Server.Models
{
    public class CallParamsModel
    {
        public string CallingUserId { get; set; }
        public string CallingUserName { get; set; }
        public string PeerId { get; set; }
        public HubConnection HubConnection { get; set; }
    }
}
