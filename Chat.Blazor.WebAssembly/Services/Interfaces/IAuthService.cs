using Chat.Blazor.WebAssembly.Models.Requests;
using Chat.Blazor.WebAssembly.Models.Responses;

namespace Chat.Blazor.WebAssembly.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RegistrationResult> Register(UserRegistrationModel registrationModel);
        Task<LoginResult> Login(UserLoginModel loginModel);
        Task<RegistrationResult> ChangePassword(ChangePasswordModel changePassModel);
        Task Logout();
    }
}