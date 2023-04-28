using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Extensions;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories.Realizations
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        private readonly ChatDbContext _dbContext;

        public GroupRepository(ChatDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsWithUsersAsync()
        {
            var groups = await _dbContext.Groups.Include(g => g.Users).Include(g=> g.Admin).AsQueryable().ToListAsync();
            return groups;
        }

        public async Task<IEnumerable<GroupInfoView>> GetGroupsByUserIdAsync(string userId, string searchText)
        {
            var info = await _dbContext.GroupsInfo.ToListAsync();
            var user = await _dbContext.Users.Include(u => u.GroupsInfo)
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId));

            var sortedGroupInfo = user.GroupsInfo.AsQueryable().SearchInGroupInfo(searchText);

            return sortedGroupInfo.AsEnumerable();
        }






    }
}
