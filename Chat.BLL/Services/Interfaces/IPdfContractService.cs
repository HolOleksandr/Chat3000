using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;

namespace Chat.BLL.Services.Interfaces
{
    public interface IPdfContractService
    {
        Task<FilterResult<PdfContractDTO>> GetAllUploadedContractsAsync(string uploaderId, SearchParameters searchParameters);
        Task<FilterResult<PdfContractDTO>> GetAllReceivedContractsAsync(string receiverId, SearchParameters searchParameters);
        Task<int> GetUnsignedContractsQty(string userId);
        Task<PdfContractDTO?> UploadPdfContractAsync(PdfContractUploadReq contractUploadRequest);
        Task UpdatePdfContractAsync(PdfContractUpdateRequest contractUpdateRequest);
        Task<PdfContractDTO?> GetFileByIdAsync(string pdfFileId);
        Task DeleteByIdAsync(string pdfFileId);
    }
}
