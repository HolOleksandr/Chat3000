
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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ChatDbContext _dbContext;

        public UserRepository(ChatDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
