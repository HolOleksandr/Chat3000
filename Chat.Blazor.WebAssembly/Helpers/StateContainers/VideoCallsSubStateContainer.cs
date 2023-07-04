namespace Chat.Blazor.WebAssembly.Helpers.StateContainers
{
    public class VideoCallsSubStateContainer
    {
        public event Func<Task>? SubCallNotificationAction;

        public async Task SubscribeForCalls()
        {
            if(SubCallNotificationAction != null)
            {
                await SubCallNotificationAction.Invoke();
            }
        }
    }
}