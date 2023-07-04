namespace Chat.Blazor.WebAssembly.Helpers.StateContainers
{
    public class PeerIdStateContainer
    {
        public event Action<string>? OnPeerIdChange;

        public void SetupPeerId(string peerId)
        {
            OnPeerIdChange?.Invoke(peerId);
        }
    }
}