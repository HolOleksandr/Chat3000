using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Chat.BLL.Services.Interfaces;

namespace Chat.BLL.Services.Implementation
{
    public class BlobManager : IBlobManager
    {
        private readonly string _blobStorageConnection;
        public BlobManager(IConfiguration configuration)
        {
            _blobStorageConnection = configuration.GetConnectionString("BlobStorageConnection");
        }

        public async Task<string> UploadAsync(IFormFile file, string containerName, string? folderPath = null)
        {
            var container = new BlobContainerClient(_blobStorageConnection, containerName.ToLower());
            var createResponce = await container.CreateIfNotExistsAsync();
            if (createResponce != null && createResponce.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(PublicAccessType.None);
            var filePath = file.FileName;
            if (!string.IsNullOrEmpty(folderPath))
            {
                filePath = Path.Combine(folderPath, file.FileName);
            }
            var blobFile = container.GetBlobClient(filePath);
            await blobFile.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
            var fileStream = file.OpenReadStream();
            await blobFile.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
            return blobFile.Uri.ToString();
        }

        public string GetBlobSasUri(string blobLinkString, int expirationHours)
        {
            var blobInfo = new BlobClient(new Uri(blobLinkString));
            BlobServiceClient blobServiceClient = new(_blobStorageConnection);
            var containerClient = blobServiceClient.GetBlobContainerClient(blobInfo.BlobContainerName);
            var blobClient = containerClient.GetBlobClient(blobInfo.Name);

            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-15),
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(expirationHours)
            };

            sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }

        public async Task DeleteBlobAsync(string blobLinkString)
        {
            var blobInfo = new BlobClient(new Uri(blobLinkString));
            BlobServiceClient blobServiceClient = new(_blobStorageConnection);
            var containerClient = blobServiceClient.GetBlobContainerClient(blobInfo.BlobContainerName);
            var blobClient = containerClient.GetBlobClient(blobInfo.Name);
            await blobClient.DeleteAsync();
        }
    }
}
