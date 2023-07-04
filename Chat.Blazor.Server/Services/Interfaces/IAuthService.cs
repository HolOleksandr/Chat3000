using Chat.Blazor.Server.Models.Requests;
using Chat.Blazor.Server.Models.Responses;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegistrationResult> Register(UserRegistrationModel registrationModel);
        Task<LoginResult> Login(UserLoginModel loginModel);
        Task<RegistrationResult> ChangePassword(ChangePasswordModel changePassModel);
        Task Logout();
    }
}
