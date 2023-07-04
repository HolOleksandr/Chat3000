using Chat.Blazor.Server.Models.DTO;
using Microsoft.AspNetCore.Components.Forms;

namespace Chat.Blazor.Server.Models.Requests
{
    public class FileUploadRequest
    {
        public UserDTO UserDTO { get; set; }
        public IBrowserFile? File { get; set; }
    }
}
