using Chat.Blazor.Server.Models;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegistrationResult> Register(UserRegistrationModel registrationModel);
        Task<LoginResult> Login(UserLoginModel loginModel);
        Task<RegistrationResult> ChangePassword(ChangePasswordModel changePassModel);

        Task Logout();
        Task<string> GetTokenAsync();
    }
}
