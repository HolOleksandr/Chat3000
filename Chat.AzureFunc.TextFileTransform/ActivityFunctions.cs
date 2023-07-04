using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Chat.AzureFunc.TextFileTransform.Helpers;
using Chat.BLL.Models.AzureFuncModels;
using Chat.BLL.Exceptions;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using Chat.AzureFunc.TextFileTransform.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Chat.AzureFunc.TextFileTransform
{
    public class ActivityFunctions
    {
        private readonly ITextInfoService _textInfoService;
        private readonly IConfiguration _configuration;
        private readonly string _blobStorageConnection;

        public ActivityFunctions(ITextInfoService textInfoService, IConfiguration configuration)
        {
            _textInfoService = textInfoService;
            _configuration = configuration;
            _blobStorageConnection = _configuration.GetConnectionString("BlobStorageConnection");

        }

        [FunctionName(nameof(CheckFileExtention))]
        public static void CheckFileExtention([ActivityTrigger] FileInfoModel fileInfo, ILogger log)
        {
            log.LogInformation("Checking file extention:  {name}.", fileInfo.FileName);
            
            var isValid = FileTypesValidator.IsValidType(fileInfo.FileName);
            if (!isValid)
            {
                log.LogError("Invalid File type");
                throw new AzureFuncException($"File extention must be one of: {FileTypesValidator.GetAvailableTypes()}");
            }
        }

        [FunctionName(nameof(UploadFile))]
        public async Task<string> UploadFile([ActivityTrigger] FileInfoModel fileInfo, ILogger log)
        {
            log.LogInformation("Uploading file:  {name}.", fileInfo.FileName);

            var container = new BlobContainerClient(_blobStorageConnection, "docfiles");

            var createResponce = await container.CreateIfNotExistsAsync();
            if (createResponce != null && createResponce.GetRawResponse().Status == 201)
                await container.SetAccessPolicyAsync(PublicAccessType.None);
            var filePath = fileInfo.UploaderId + "/" + fileInfo.FileName;
            var blobClient = container.GetBlobClient(filePath);
            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            using (MemoryStream stream = new(fileInfo.Data))
            {
                blobClient.Upload(stream, true);
            }
            log.LogInformation("File uploaded.");
            return blobClient.Uri.ToString();
        }

        [FunctionName(nameof(GetFileText))]
        public string GetFileText([ActivityTrigger] string blobUrl, ILogger log)
        {
            log.LogInformation("Extracting text");
            var blobInfo = new BlobClient(new Uri(blobUrl));
            BlobServiceClient blobServiceClient = new(_blobStorageConnection);
            var containerClient = blobServiceClient.GetBlobContainerClient(blobInfo.BlobContainerName);
            var blobClient = containerClient.GetBlobClient(blobInfo.Name);
            using (MemoryStream stream = new MemoryStream())
            {
                blobClient.DownloadTo(stream);
                stream.Position = 0;
                using (WordprocessingDocument document = WordprocessingDocument.Open(stream, false))
                {
                    string extractedText = document.MainDocumentPart.Document.Body.InnerText;
                    return extractedText;
                }
            }
        }

        [FunctionName(nameof(WriteTextToDb))]
        public async Task WriteTextToDb([ActivityTrigger] DocFileText textInfoModel, ILogger log)
        {
            log.LogInformation("Adding text");
            await _textInfoService.AddAsync(textInfoModel);
        }
    }
}
