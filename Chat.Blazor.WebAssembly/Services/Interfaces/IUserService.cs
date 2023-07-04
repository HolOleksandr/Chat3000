using Chat.Blazor.WebAssembly.Models.DTO;
using Chat.Blazor.WebAssembly.Models.Paging;
using Chat.Blazor.WebAssembly.Models.Requests;
using Chat.Blazor.WebAssembly.Models.Responses;

namespace Chat.Blazor.WebAssembly.Services.Interfaces
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