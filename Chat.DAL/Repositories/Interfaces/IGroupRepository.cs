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
        Task<IEnumerable<Group>> GetGroupsByUserIdAsync(string userId, string searchText);
        Task<IEnumerable<Group>> GetGroupsSortedByUsersQtyAsync(string userId, bool SortAsc, string searchText);
        Task<IEnumerable<Group>> GetGroupsSortedByAdminEmailAsync(string userId, bool SortAsc, string searchText);

    }
}
