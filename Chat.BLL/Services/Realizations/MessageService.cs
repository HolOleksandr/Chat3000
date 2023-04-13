using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.Repositories.Realizations;
using Chat.DAL.UoW.Interface;
using Chat.DAL.UoW.Realization;

namespace Chat.BLL.Services.Realizations
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MessageDTO> GetMessageByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<IMessageRepository>();
            repo.CreateMessageTest(new Message());


            return new MessageDTO();
        } 
    }
}
