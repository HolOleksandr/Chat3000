using Chat.Blazor.Server.Models.DTO;

namespace Chat.Blazor.Server.Helpers.StateContainers
{
    public class GroupInfoStateContainer
    {
        public GroupInfoViewDTO? Value { get; set; }
        public event Action? OnStateChange;
        public void SetValue(GroupInfoViewDTO value)
        {
            this.Value = value;
            NotifyStateChanged();

        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
