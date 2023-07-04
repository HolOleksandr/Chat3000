using Microsoft.AspNetCore.Components.Forms;
using System.Drawing;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageInBase64(IBrowserFile file);
        Task<IFormFile> ResizeImageAsync(IBrowserFile file, int width, int height);
    }
}
