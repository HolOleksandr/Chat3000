using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;

namespace Chat.BLL.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDTO>> GetGroupsAsync();
        Task<FilterResult<GroupInfoViewDTO>> GetGroupsByUserEmailAsync(string userEmail, SearchParameters searchParameters);
        Task<GroupInfoViewDTO> GetGroupViewByIdAsync(string groupId);
        Task CreateNewGroup(CreateGroupRequest groupRequest);
    }
}
