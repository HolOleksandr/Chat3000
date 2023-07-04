using Chat.Blazor.Server.Models.DTO;
using Microsoft.AspNetCore.Components.Forms;

namespace Chat.Blazor.Server.Models.Requests
{
    public class PdfContractUpdateRequest
    {
        public PdfContractDTO PdfContractInfo { get; set; } = null!;
        public IBrowserFile? File { get; set; }
    }
}
