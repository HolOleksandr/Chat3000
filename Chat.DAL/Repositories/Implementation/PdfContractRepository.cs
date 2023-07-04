using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Extensions;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Repositories.Implementation
{
    public class PdfContractRepository : BaseRepository<PdfContract>, IPdfContractRepository
    {
        private readonly ChatDbContext _dbContext;
        public PdfContractRepository(ChatDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }            

        public async Task<IEnumerable<PdfContract>?> GetAllUploadedContracts(string uploaderId, string searchText)
        {
            var contracts = await _dbContext.PdfContract
                .Include(c => c.Uploader)
                .Include(c => c.Receiver)
                .Where(c => c.UploaderId == uploaderId)
                .SearchInContractsInfo(searchText)
                .ToListAsync();

            return contracts;
        }
        public async Task<IEnumerable<PdfContract>?> GetAllReceivedContracts(string receiverId, string searchText)
        {
            var contracts = await _dbContext.PdfContract
                .Include(c => c.Uploader)
                .Include(c => c.Receiver)
                .Where(c => c.ReceiverId == receiverId)
                .SearchInContractsInfo(searchText)
                .ToListAsync();

            return contracts;
        }

        public async Task<int> GetUnsignedContractsQty(string userId)
        {
            var contractsQty = await _dbContext.PdfContract.Where(c => c.ReceiverId == userId && c.IsSigned == false).CountAsync();
            return contractsQty;
        }
    }
}
