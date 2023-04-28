using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using System.Drawing;

namespace Chat.Blazor.Server.Services.Realization
{
    public class ImageService : IImageService
    {
        public async Task<string> SaveImageInBase64(IBrowserFile file)
        {
            var contentBytes = new byte[file.Size];
            using var stream = file.OpenReadStream();
            await stream.ReadAsync(contentBytes, 0, contentBytes.Length);

            var contentBase64Str = Convert.ToBase64String(contentBytes);
            var contentType = file.ContentType;
            var ImageUrl = "data:" + contentType + ";base64," + contentBase64Str;

            return ImageUrl;
        }
    }
}
