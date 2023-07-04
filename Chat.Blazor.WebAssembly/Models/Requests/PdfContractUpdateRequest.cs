using Chat.Blazor.WebAssembly.Models.DTO;
using Microsoft.AspNetCore.Components.Forms;

namespace Chat.Blazor.WebAssembly.Models.Requests
{
    public class PdfContractUpdateRequest
    {
        public PdfContractDTO PdfContractInfo { get; set; } = null!;
        public IBrowserFile? File { get; set; }
    }
}
