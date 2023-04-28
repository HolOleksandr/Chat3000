using Chat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<IEnumerable<Group>> GetAllGroupsWithUsersAsync();
        Task<IEnumerable<GroupInfoView>> GetGroupsByUserIdAsync(string userId, string searchText);
        

    }
}
