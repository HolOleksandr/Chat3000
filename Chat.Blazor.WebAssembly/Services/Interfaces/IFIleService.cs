using Chat.Blazor.WebAssembly.Models.Requests;
using Chat.Blazor.WebAssembly.Models.Responses;

namespace Chat.Blazor.WebAssembly.Services.Interfaces
{
    public interface IFIleService
    {
        Task<FileConvertorResponse> ConvertTextFileAsync(FileUploadRequest fileUploadRequest);
    }
}