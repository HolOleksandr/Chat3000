using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Exceptions;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Interface;

namespace Chat.BLL.Services.Implementation
{
    public class PdfContractService : IPdfContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobManager _blobManager;
        private readonly IMapper _mapper;
        private readonly IPdfContractRepository _pdfContractRepository;

        public PdfContractService(IUnitOfWork unitOfWork, IBlobManager blobManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobManager = blobManager;
            _pdfContractRepository = _unitOfWork.GetRepository<IPdfContractRepository>();
        }

        public async Task<FilterResult<PdfContractDTO>> GetAllUploadedContractsAsync(string uploaderId, SearchParameters searchParameters)
        {
            var uploadedContracts = await _pdfContractRepository.GetAllUploadedContracts(uploaderId, searchParameters.FilterQuery);

            var mappedContracts = _mapper.Map<IEnumerable<PdfContractDTO>>(uploadedContracts);
            return await FilterResult<PdfContractDTO>.CreateAsync(
                 mappedContracts,
                 searchParameters.PageIndex,
                 searchParameters.PageSize,
                 searchParameters.SortColumn,
                 searchParameters.SortOrder,
                 searchParameters.FilterQuery
                 );
        }

        public async Task<FilterResult<PdfContractDTO>> GetAllReceivedContractsAsync(string receiverId, SearchParameters searchParameters)
        {
            var receivedContracts = await _pdfContractRepository.GetAllReceivedContracts(receiverId, searchParameters.FilterQuery);

            var mappedContracts = _mapper.Map<IEnumerable<PdfContractDTO>>(receivedContracts);
            return await FilterResult<PdfContractDTO>.CreateAsync(
                 mappedContracts,
                 searchParameters.PageIndex,
                 searchParameters.PageSize,
                 searchParameters.SortColumn,
                 searchParameters.SortOrder,
                 searchParameters.FilterQuery
                 );
        }

        public async Task<PdfContractDTO?> UploadPdfContractAsync(PdfContractUploadReq contractUploadRequest)
        {
            var userUpdateModel = contractUploadRequest.UserDTO;
            var user = await _unitOfWork.GetRepository<IUserRepository>().GetUserByStringIdAsync(userUpdateModel.Id)
                ?? throw new ChatException("User is not exist");

            if (contractUploadRequest.File != null)
            {
                var uploadDate = DateTime.Now;
                var folderPath = Path.Combine(user.Id, uploadDate.ToString());
                var fileUrlStr = await _blobManager.UploadAsync(contractUploadRequest.File, "pdfcontracts", folderPath);
                var pdfInfo = new PdfContract()
                {
                    FileName = contractUploadRequest.File.FileName,
                    FileUrl = fileUrlStr,
                    UploaderId = user.Id,
                    IsSigned = false,
                    SignFieldsNum = 0,
                    UploadDate = uploadDate
                };

                await _pdfContractRepository.AddAsync(pdfInfo);
                await _unitOfWork.SaveAsync();

                var pdfInfoDto = _mapper.Map<PdfContractDTO>(pdfInfo);

                return pdfInfoDto;
            }
            return null;
        }

        public async Task UpdatePdfContractAsync(PdfContractUpdateRequest contractUpdateRequest)
        {
            var pdfInfo = await _pdfContractRepository.GetByIdAsync(contractUpdateRequest.PdfContractInfo.Id)
                ?? throw new ChatException("File is not exist");
            if (contractUpdateRequest.File == null)
            {
                throw new ChatException("File is not exist");
            }
            else
            {
                var folderPath = Path.Combine(pdfInfo.UploaderId, pdfInfo.UploadDate.ToString());
                var fileUrlStr = await _blobManager.UploadAsync(contractUpdateRequest.File, "pdfcontracts", folderPath);
                pdfInfo.ReceiverId = contractUpdateRequest.PdfContractInfo.ReceiverId;
                pdfInfo.IsPsPdf = contractUpdateRequest.PdfContractInfo.IsPsPdf;
                pdfInfo.IsSyncF = contractUpdateRequest.PdfContractInfo.IsSyncF;
                pdfInfo.IsSigned = contractUpdateRequest.PdfContractInfo.IsSigned;
                pdfInfo.SignDate = contractUpdateRequest.PdfContractInfo.SignDate;
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<int> GetUnsignedContractsQty(string userId)
        {
            var unsignedContractsQty = await _pdfContractRepository.GetUnsignedContractsQty(userId);
            return unsignedContractsQty;
        }

        public async Task<PdfContractDTO?> GetFileByIdAsync(string pdfFileId)
        {
            if (Guid.TryParse(pdfFileId, out Guid guidId))
            {
                var pdfInfo = await _pdfContractRepository.GetByIdAsync(guidId)
                 ?? throw new ChatException("File is not exist");
                var pdfInfoDto = _mapper.Map<PdfContractDTO>(pdfInfo);
                return pdfInfoDto;
            }
            else
            {
                throw new ChatException("File is not exist");
            }
        }

        public async Task DeleteByIdAsync(string pdfFileId)
        {
            try
            {
                var pdfInfo = await GetFileByIdAsync(pdfFileId);
                if (pdfInfo != null && !string.IsNullOrEmpty(pdfInfo.FileUrl))
                {
                    await _pdfContractRepository.Delete(pdfInfo.Id);
                    await _blobManager.DeleteBlobAsync(pdfInfo.FileUrl);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception)
            {
                throw new ChatException("File can't be deleted now");
            }
        }
    }
}
