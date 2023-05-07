using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.DAL.Repositories.Implementation
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly ChatDbContext _dbContext;

        public MessageRepository(ChatDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Message>> GetAllByGroupIdAsync(int groupId)
        {
            var messages = await Task.FromResult(_dbContext.Messages
                    .Where(m => m.GroupId == groupId)
                    .Include(m => m.Receiver)
                    .Include(m => m.Sender)
                    .OrderBy(m => m.SendDate)
                    .AsNoTracking());

            return messages.AsEnumerable();
        }
    }
}
