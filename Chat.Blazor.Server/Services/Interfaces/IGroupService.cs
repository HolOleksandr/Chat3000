using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Paging;
using Chat.Blazor.Server.Models.Requests;
using Chat.Blazor.Server.Models.Responses;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IGroupService
    {
        Task<PagingResponse<GroupInfoViewDTO>> GetAllUserGroupsWithSortAsync(string userEmail, string queryParams);
        Task<RegistrationResult> CreateNewGroup(CreateGroupRequest _createGroupRequest);
        Task<GroupInfoViewDTO> GetGroupByIdAsync(int groupId);
    }
}
