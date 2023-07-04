using Chat.DAL.Entities;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IPdfContractRepository : IBaseRepository<PdfContract>
    {
        Task<IEnumerable<PdfContract>?> GetAllUploadedContracts(string uploaderId, string searchText);
        Task<IEnumerable<PdfContract>?> GetAllReceivedContracts(string receiverId, string searchText);
        Task<int> GetUnsignedContractsQty(string userId);
    }
}
