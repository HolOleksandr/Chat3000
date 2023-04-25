using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Paging;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagingResponse<UserDTO>> GetAllUsersWithSortAsync(string queryParams);
        Task<RegistrationResult> UpdateUserAsync(UserDTO updateUserModel);
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<IEnumerable<string>> GetUsersEmailsExcepMaker(string makerEmail);
        Task<IEnumerable<UserShortInfoDTO>> GetUsersShortInfoExceptMaker(string makerEmail);
    }
}
