using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Paging;
using Chat.Blazor.Server.Models.Requests;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IGroupService
    {
        Task<PagingResponse<GroupDTO>> GetAllUserGroupsWithSortAsync(string userEmail, string queryParams);
        Task<RegistrationResult> CreateNewGroup(CreateGroupRequest _createGroupRequest);
    }
}
