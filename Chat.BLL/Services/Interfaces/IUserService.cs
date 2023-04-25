using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<FilterResult<UserDTO>> GetAllUsersAsync(SearchParameters searchParameters);
        Task UpdateUserInfoAsync(UserDTO userupdateModel);
        Task<UserDTO?> GetUserByIdAsync(string userId);
        Task<IEnumerable<string>> GetEmailsExceptMakerAsync(string makerEmail);
        Task<IEnumerable<UserShortInfoDTO>> GetUsersShortInfoExceptMakerAsync(string? makerEmail); 
    }
}
