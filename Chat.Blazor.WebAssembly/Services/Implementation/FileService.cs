using Chat.Blazor.WebAssembly.Helpers.Interfaces;
using Chat.Blazor.WebAssembly.Models.Requests;
using Chat.Blazor.WebAssembly.Models.Responses;
using Chat.Blazor.WebAssembly.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Chat.Blazor.WebAssembly.Services.Implementation
{
    public class FileService : IFIleService
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomHttpClient _customHttpClient;

        public FileService(ICustomHttpClient customHttpClient, IConfiguration configuration)
        {
            _customHttpClient = customHttpClient;
            _configuration = configuration;
        }

        public async Task<FileConvertorResponse> ConvertTextFileAsync(FileUploadRequest fileUploadRequest)
        {
            var _baseUrl = _configuration["FileTransformAzureFuncUrl"];
            using var form = new MultipartFormDataContent();
            var json = JsonSerializer.Serialize(fileUploadRequest.UserDTO);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            form.Add(jsonContent, "UserDTO");

            if (fileUploadRequest.File != null)
            {
                using var memoryStream = new MemoryStream();
                await fileUploadRequest.File.OpenReadStream(fileUploadRequest.File.Size).CopyToAsync(memoryStream);
                var fileContent = new ByteArrayContent(memoryStream.ToArray());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(fileUploadRequest.File.ContentType);
                form.Add(fileContent, "textFile", fileUploadRequest.File.Name);
            }

            var result = await _customHttpClient.PostContentWithTokenAsync(_baseUrl, form);

            if (result.IsSuccessStatusCode)
            {
                DurableFuncStatusResponse funcResult;
                string durableFuncResponse = await result.Content.ReadAsStringAsync();
                var stream = await result.Content.ReadAsStreamAsync();

                var funcDetail = JsonSerializer.Deserialize<DurableFuncDetails>(durableFuncResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var statusQuery = funcDetail.StatusQueryGetUri;
                do 
                {
                    var status = await _customHttpClient.GetWithTokenAsync(statusQuery);
                    var statusInfo = await status.Content.ReadAsStringAsync();
                    funcResult = JsonSerializer.Deserialize<DurableFuncStatusResponse>(statusInfo, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Task.Delay(3000).Wait();
                } while (funcResult.RuntimeStatus != "Completed" && funcResult.Output == null);
                

                return funcResult.Output;
            }
            return new FileConvertorResponse { Success = false, Text = null };
        }
    }
}