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

        public async Task<IEnumerable<Group>> GetGroupsByUserIdAsync(string userId, string searchText)
        {
            var user = await _dbContext.Users
                .Include(u => u.Groups)
                .ThenInclude(g => g.Users)
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId));

            var sortedGroups = user.Groups.AsQueryable().SearchInGroups(searchText);

            return sortedGroups.AsEnumerable();
        }

        public async Task<IEnumerable<Group>> GetGroupsSortedByUsersQtyAsync(string userId, bool sortAsc, string searchText)
        {
            var _groups = (await GetGroupsByUserIdAsync(userId, searchText)).AsQueryable();

            if (sortAsc)
            {
                _groups = _groups.OrderBy(g => g.Users.Count);
            }
            else
            {
                _groups = _groups.OrderByDescending(g => g.Users.Count);
            }
            return await Task.FromResult(_groups.AsEnumerable());
        }

        public async Task<IEnumerable<Group>> GetGroupsSortedByAdminEmailAsync(string userId, bool sortAsc, string searchText)
        {
            var user = await _dbContext.Users
                .Include(u => u.Groups)
                .ThenInclude(g => g.Users)
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId));
                
            var _groups = user.Groups.AsQueryable().SearchInGroups(searchText);

            if (sortAsc)
            {
                _groups = _groups.OrderBy(g => g.Admin.Email);
            }
            else
            {
                _groups = _groups.OrderByDescending(g => g.Admin.Email);
            }

            return await Task.FromResult(_groups.AsEnumerable());
        }





    }
}
