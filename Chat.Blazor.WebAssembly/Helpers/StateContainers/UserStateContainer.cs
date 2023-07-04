using Chat.Blazor.WebAssembly.Models.DTO;

namespace Chat.Blazor.WebAssembly.Helpers.StateContainers
{
    public class UserStateContainer
    {
        public UserDTO? Value { get; set; }
        public event Action? OnStateChange;
        public void SetValue (UserDTO value)
        {
            this.Value = value;
            NotifyStateChanged();

        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}