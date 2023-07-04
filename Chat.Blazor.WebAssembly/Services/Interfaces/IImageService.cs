using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Chat.Blazor.WebAssembly.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageInBase64(IBrowserFile file);
        Task<IFormFile> ResizeImageAsync(IBrowserFile file, int width, int height);
    }
}