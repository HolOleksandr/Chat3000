using Chat.BLL.Models;
using Chat.BLL.Models.Requests;
using Microsoft.AspNetCore.Identity;

namespace Chat.BLL.Services.Interfaces
{
    public interface IUserAuthService
    {
        Task<string> CreateTokenAsync();
        Task<IdentityResult> RegisterUserAsync(UserRegistrationModel userModel);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
        Task<bool> ValidateUserAsync(UserLoginModel userModel);
    }
}
