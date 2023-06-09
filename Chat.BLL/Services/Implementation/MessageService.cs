﻿using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Interface;

namespace Chat.BLL.Services.Implementation
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

        public async Task AddNewMessageAsync(MessageDTO message)
        {
            message.SendDate = DateTime.Now;
            message.Sender = null;
            var messageEntity = _mapper.Map<Message>(message);
            await _unitOfWork.GetRepository<IMessageRepository>().AddAsync(messageEntity);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<MessageDTO>> GetAllMessagesByGroupIdasync(int groupId)
        {
            var messages = await _unitOfWork.GetRepository<IMessageRepository>().GetAllByGroupIdAsync(groupId);
            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }
    }
}
