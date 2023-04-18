namespace Chat.Blazor.Server.Helpers.Interfaces
{
    public interface ICustomHttpClient
    {
        Task<HttpResponseMessage> GetWithTokenAsync(string url);
        Task<HttpResponseMessage> PostWithTokenAsync(string url, HttpContent content);
    }
}
