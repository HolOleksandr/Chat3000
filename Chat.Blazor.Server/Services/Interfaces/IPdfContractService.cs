using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Paging;
using Chat.Blazor.Server.Models.Requests;
using Chat.Blazor.Server.Models.Responses;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IPdfContractService
    {
        Task<PagingResponse<PdfContractDTO>> GetAllUploadedContractsAsync(string userId, string queryParams);
        Task<PagingResponse<PdfContractDTO>> GetAllReceivedContractsAsync(string userId, string queryParams);
        Task<int> GetUnsignedContractsQty(string userId);
        Task<PdfContractDTO?> UploadPdfContractAsync(PdfContractUploadReq pdfContractUploadRequest);
        Task<RequestResult> SendContractForSignAsync(PdfContractDTO pdfContract, byte[] byteContent, string fileName);
        Task<PdfContractDTO> GetPdfContractInfoById(string pdfFileId);
        Task<RequestResult> DeleteContractAsync(string fileId);
    }
}
