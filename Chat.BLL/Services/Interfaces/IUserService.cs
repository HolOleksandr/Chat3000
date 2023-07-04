using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;

namespace Chat.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<FilterResult<UserDTO>> GetAllUsersAsync(SearchParameters searchParameters);
        Task UpdateUserInfoAsync(UserUpdateRequest userUpdateRequest);
        Task<UserDTO?> GetUserByIdAsync(string userId);
        Task<IEnumerable<string>> GetEmailsExceptMakerAsync(string makerEmail);
        Task<IEnumerable<UserShortInfoDTO>> GetUsersShortInfoExceptMakerAsync(string? makerEmail); 
    }
}
