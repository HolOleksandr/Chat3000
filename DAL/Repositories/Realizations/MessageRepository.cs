using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Realizations
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        private readonly ChatDbContext _dbContext;

        public MessageRepository(ChatDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
