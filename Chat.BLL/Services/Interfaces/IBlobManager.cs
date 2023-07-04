using Microsoft.AspNetCore.Http;

namespace Chat.BLL.Services.Interfaces
{
    public interface IBlobManager
    {
        Task<string> UploadAsync(IFormFile file, string containerName, string? folderPath = null);
        string GetBlobSasUri(string blobLinkString, int expirationHours);
        Task DeleteBlobAsync(string blobLinkString);
    }
}
