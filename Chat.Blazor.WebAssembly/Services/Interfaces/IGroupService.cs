using Chat.Blazor.WebAssembly.Models.DTO;
using Chat.Blazor.WebAssembly.Models.Paging;
using Chat.Blazor.WebAssembly.Models.Requests;
using Chat.Blazor.WebAssembly.Models.Responses;

namespace Chat.Blazor.WebAssembly.Services.Interfaces
{
    public interface IGroupService
    {
        Task<PagingResponse<GroupInfoViewDTO>> GetAllUserGroupsWithSortAsync(string userEmail, string queryParams);
        Task<RegistrationResult> CreateNewGroup(CreateGroupRequest _createGroupRequest);
        Task<GroupInfoViewDTO> GetGroupByIdAsync(int groupId);
    }
}