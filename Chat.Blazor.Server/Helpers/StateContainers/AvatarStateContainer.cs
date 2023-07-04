namespace Chat.Blazor.Server.Helpers.StateContainers
{
    public class AvatarStateContainer
    {
        public event Func<Task>? OnAvatarChange;

        public async Task ChangeAvatar()
        {
            if (OnAvatarChange != null)
            {
                await OnAvatarChange.Invoke();
            }
        }
    }
}
