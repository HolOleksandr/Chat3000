using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;

namespace Chat.DAL.Repositories.Implementation
{
    public class UserGroupRepository : BaseRepository<UserGroup>, IUserGroupRepository
    {
        private readonly ChatDbContext _dbContext;

        public UserGroupRepository(ChatDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
