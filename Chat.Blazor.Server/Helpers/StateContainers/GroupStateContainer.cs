using Chat.Blazor.Server.Models.DTO;

namespace Chat.Blazor.Server.Helpers.StateContainers
{
    public class GroupStateContainer
    {
        public GroupDTO? Value { get; set; }
        public event Action? OnStateChange;
        public void SetValue(GroupDTO value)
        {
            this.Value = value;
            NotifyStateChanged();

        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
