namespace Chat.Blazor.Server.Helpers.StateContainers
{
    public class AvatarStateContainer
    {
        public event Action<string> OnAvatarChange;

        public void ChangeAvatar(string avatar)
        {
            OnAvatarChange?.Invoke(avatar);
        }
    }
}
