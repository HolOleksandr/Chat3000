namespace Chat.Blazor.Server.Helpers.Interfaces
{
    public interface ICustomHttpClient
    {
        Task<HttpResponseMessage> GetWithTokenAsync(string url);
        Task<HttpResponseMessage> PostWithTokenAsync<T>(string url, T content) where T : class;
        Task<HttpResponseMessage> PostContentWithTokenAsync(string url, MultipartFormDataContent content);
        Task<HttpResponseMessage> DeleteWithTokenAsync(string url);
    }
}
