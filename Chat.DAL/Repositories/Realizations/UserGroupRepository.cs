using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories.Realizations
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
