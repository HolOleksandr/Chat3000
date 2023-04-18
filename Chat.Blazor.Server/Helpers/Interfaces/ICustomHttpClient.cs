namespace Chat.Blazor.Server.Helpers.Interfaces
{
    public interface ICustomHttpClient
    {
        Task<HttpResponseMessage> GetWithTokenAsync(string url);
        Task<HttpResponseMessage> PostWithTokenAsync(string url, HttpContent content);

        Task<HttpResponseMessage> SendWithTokenAsync(HttpMethod method, string url, HttpContent content = null);
    }
}
