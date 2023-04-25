using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Paging;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IGroupService
    {
        Task<PagingResponse<GroupDTO>> GetAllUserGroupsWithSortAsync(string userEmail, string queryParams);
        Task<RegistrationResult> CreateNewGroup(IEnumerable<string> emails);
    }
}
