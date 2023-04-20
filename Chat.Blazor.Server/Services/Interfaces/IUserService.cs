using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.Paging;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagingResponse<UserDTO>> GetAllUsersWithSortAsync(string queryParams);
    }
}
