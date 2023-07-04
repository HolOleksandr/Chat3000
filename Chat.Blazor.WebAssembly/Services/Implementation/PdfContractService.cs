using Chat.Blazor.WebAssembly.Helpers.Interfaces;
using Chat.Blazor.WebAssembly.Models;
using Chat.Blazor.WebAssembly.Models.DTO;
using Chat.Blazor.WebAssembly.Models.Paging;
using Chat.Blazor.WebAssembly.Models.Requests;
using Chat.Blazor.WebAssembly.Models.Responses;
using Chat.Blazor.WebAssembly.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Chat.Blazor.WebAssembly.Services.Implementation
{
    public class PdfContractService : IPdfContractService
    {
        private readonly string _baseUrl = "";
        private readonly IConfiguration _configuration;
        private readonly ICustomHttpClient _customHttpClient;

        public PdfContractService(ICustomHttpClient customHttpClient, IConfiguration configuration)
        {
            _customHttpClient = customHttpClient;
            _configuration = configuration;
            _baseUrl = _configuration["ChatApi"];
        }

        public async Task<PagingResponse<PdfContractDTO>> GetAllUploadedContractsAsync(string userId, string queryParams)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/pdfcontract/uploaded/" + userId + queryParams));
            var stream = await response.Content.ReadAsStreamAsync();
            var pagingResponse = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<PagingResponse<PdfContractDTO>>(stream);

            return pagingResponse;
        }

        public async Task<PagingResponse<PdfContractDTO>> GetAllReceivedContractsAsync(string userId, string queryParams)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/pdfcontract/received/" + userId + queryParams));
            var stream = await response.Content.ReadAsStreamAsync();
            var pagingResponse = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<PagingResponse<PdfContractDTO>>(stream);

            return pagingResponse;
        }

        public async Task<int> GetUnsignedContractsQty(string userId)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/pdfcontract/unsigned/" + userId));
            var stream = await response.Content.ReadAsStreamAsync();
            var unsignedContracts = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<int>(stream);

            return unsignedContracts;
        }

        public async Task<PdfContractDTO?> UploadPdfContractAsync(PdfContractUploadReq pdfContractUploadRequest)
        {
            using var form = new MultipartFormDataContent();
            var json = JsonSerializer.Serialize(pdfContractUploadRequest.UserDTO);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            form.Add(jsonContent, "UserDTO");

            if (pdfContractUploadRequest.File != null)
            {
                using var memoryStream = new MemoryStream();
                await pdfContractUploadRequest.File.OpenReadStream(pdfContractUploadRequest.File.Size).CopyToAsync(memoryStream);
                var fileContent = new ByteArrayContent(memoryStream.ToArray());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "file", pdfContractUploadRequest.File.Name);
            }

            var result = await _customHttpClient.PostContentWithTokenAsync(_baseUrl + "api/pdfcontract/upload", form);

            if (result.IsSuccessStatusCode)
            { 
                var stream = await result.Content.ReadAsStreamAsync();
                var uploadingResult = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<PdfContractDTO>(stream);
                return uploadingResult;
            }

            return null;
        }

        public async Task<RequestResult> SendContractForSignAsync(PdfContractDTO pdfContract, byte[] byteContent, string fileName)
        {
            using var form = new MultipartFormDataContent();
            var json = JsonSerializer.Serialize(pdfContract);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            form.Add(jsonContent, "PdfContractInfo");

            if (byteContent != null && byteContent.Length > 0)
            {
                var fileContent = new ByteArrayContent(byteContent);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                form.Add(fileContent, "file", fileName);
            }

            var result = await _customHttpClient.PostContentWithTokenAsync(_baseUrl + "api/pdfcontract/sendcontract", form);

            if (result.IsSuccessStatusCode)
                return new RequestResult { Success = true, Errors = null };

            var stream = await result.Content.ReadAsStreamAsync();
                var uploadingResult = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<Error>(stream);
            return new RequestResult { Success = false, Errors = new List<string>() { uploadingResult.Message } };
        }

        public async Task<PdfContractDTO> GetPdfContractInfoById(string pdfFileId)
        {
            using var response = await _customHttpClient
                .GetWithTokenAsync((_baseUrl + "api/pdfcontract/id/" + pdfFileId));
            var stream = await response.Content.ReadAsStreamAsync();
            var fileDto = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<PdfContractDTO>(stream);
            return fileDto;
        }

        public async Task<RequestResult> DeleteContractAsync(string fileId)
        {
            
            var result = await _customHttpClient.DeleteWithTokenAsync(_baseUrl + "api/pdfcontract/id/" + fileId);

            if (result.IsSuccessStatusCode)
                return new RequestResult { Success = true, Errors = null };

            var stream = await result.Content.ReadAsStreamAsync();
            var uploadingResult = await ServiceStack.Text.JsonSerializer.DeserializeFromStreamAsync<Error>(stream);
            return new RequestResult { Success = false, Errors = new List<string>() { uploadingResult.Message } };
        }
    }
}