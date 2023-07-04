using Chat.Blazor.WebAssembly.Models;

namespace Chat.Blazor.WebAssembly.Helpers.StateContainers
{
    public class CallParamsStateContainer
    {
        public CallParamsModel? Value { get; set; }
        public event Action? OnStateChange;
        public void SetValue(CallParamsModel? value)
        {
            this.Value = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
