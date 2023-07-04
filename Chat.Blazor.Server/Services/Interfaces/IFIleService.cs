using Chat.Blazor.Server.Models.Requests;
using Chat.Blazor.Server.Models.Responses;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IFIleService
    {
        Task<FileConvertorResponse> ConvertTextFileAsync(FileUploadRequest fileUploadRequest);
    }
}
