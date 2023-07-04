using Chat.Blazor.WebAssembly.Models.DTO;
using Microsoft.AspNetCore.Components.Forms;

namespace Chat.Blazor.WebAssembly.Models.Requests
{
    public class FileUploadRequest
    {
        public UserDTO? UserDTO { get; set; }
        public IBrowserFile? File { get; set; }
    }
}