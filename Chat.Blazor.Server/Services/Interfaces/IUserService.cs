using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Paging;
using Chat.Blazor.Server.Models.Requests;
using Chat.Blazor.Server.Models.Responses;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagingResponse<UserDTO>> GetAllUsersWithSortAsync(string queryParams);
        Task<RegistrationResult> UpdateUserAsync(UserUpdateRequest userUpdateRequest);
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<IEnumerable<string>> GetUsersEmailsExcepMaker(string makerEmail);
        Task<IEnumerable<UserShortInfoDTO>> GetUsersShortInfoExceptMaker(string makerEmail);
    }
}
