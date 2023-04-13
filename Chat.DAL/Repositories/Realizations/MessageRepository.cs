using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;

namespace Chat.DAL.Repositories.Realizations
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly ChatDbContext _dbContext;

        public MessageRepository(ChatDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Message CreateMessageTest(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
