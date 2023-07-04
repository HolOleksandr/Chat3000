using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Blazor.Server.Helpers.StateContainers.Hubs
{
    public class VideoCallHubStateContainer
    {
        public HubConnection? Value { get; set; }
        public event Action? OnStateChange;
        public void SetValue(HubConnection value)
        {
            this.Value = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
