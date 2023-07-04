﻿using Chat.DAL.Entities;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetAllByGroupIdAsync(int groupId);
    }
}
