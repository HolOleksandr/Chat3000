using Chat.DAL.Entities;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<IEnumerable<Group>> GetAllGroupsWithUsersAsync();
        Task<IEnumerable<GroupInfoView>?> GetGroupsByUserEmailAsync(string userEmail, string searchText);
        Task<GroupInfoView?> GetGroupViewById(int groupId);
    }
}
