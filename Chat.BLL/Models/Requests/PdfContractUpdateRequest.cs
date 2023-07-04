using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Chat.BLL.Models.Requests
{
    public class PdfContractUpdateRequest
    {
        [ModelBinder(BinderType = typeof(FormDataJsonBinder))]
        public PdfContractDTO PdfContractInfo { get; set; } = null!;
        public IFormFile? File { get; set; }
    }
}
