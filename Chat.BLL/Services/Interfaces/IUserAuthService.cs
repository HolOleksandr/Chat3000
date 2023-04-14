using Chat.DAL.Entities.AuthModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
