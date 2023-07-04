using Chat.Blazor.Server.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Chat.Blazor.Server.Services.Implementation
{
    public class ImageService : IImageService
    {
        public async Task<string> SaveImageInBase64(IBrowserFile file)
        {
            try
            {
                var contentBytes = new byte[file.Size];
                using var stream = file.OpenReadStream(file.Size);
                await stream.ReadAsync(contentBytes, 0, contentBytes.Length);

                var contentBase64Str = Convert.ToBase64String(contentBytes);
                var contentType = file.ContentType;
                var ImageUrl = "data:" + contentType + ";base64," + contentBase64Str;
                return ImageUrl;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }

        public async Task<IFormFile> ResizeImageAsync(IBrowserFile file, int width, int height)
        {
            var memoryStream = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using (var outputImage = await Image.LoadAsync(memoryStream))
            {
                outputImage.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max
                }));

                using (var resizedImageStream = new MemoryStream())
                {
                    await outputImage.SaveAsync(resizedImageStream, new JpegEncoder());

                    resizedImageStream.Position = 0;
                    var formFile = new FormFile(resizedImageStream, 0, resizedImageStream.Length, "name", "avatar")
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = file.ContentType,
                        ContentDisposition = $"inline; filename=avatar"
                    };
                    return formFile;
                }
            }
        }
    }
}
