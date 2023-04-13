using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities.AuthModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IValidator<UserLoginModel> _userLoginValidator;
        private readonly IValidator<UserRegistrationModel> _userRegistrationValidator;
        private readonly IUserAuthService _userAuthService;

        public AccountController(IUserAuthService userAuthService,
            IValidator<UserLoginModel> userLoginValidator,
            IValidator<UserRegistrationModel> userRegistrationValidator)
        {
            _userAuthService = userAuthService;
            _userLoginValidator = userLoginValidator;
            _userRegistrationValidator = userRegistrationValidator;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationModel userModel)
        {
            var validation = await _userRegistrationValidator.ValidateAsync(userModel);
            if (!validation.IsValid)
                return StatusCode(400, validation.Errors.Select(e => e.ErrorMessage));

            var result = await _userAuthService.RegisterUserAsync(userModel);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResult { Success = false, Errors = errors });
            }
            return Accepted(new RegistrationResult { Success = true });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginModel user)
        {
            var validation = await _userLoginValidator.ValidateAsync(user);
            if (!validation.IsValid)
                return StatusCode(400, validation.Errors.Select(e => e.ErrorMessage));


            if (!await _userAuthService.ValidateUserAsync(user))
            {
                return Unauthorized(new LoginResult()
                {
                    Success = false,
                    Message = "Invalid Email or Password."
                });
            }
            var token = await _userAuthService.CreateTokenAsync();
            return Accepted(new LoginResult()
            {
                Success = true,
                Message = "Login successful",
                Token = token
            });
        }
    }
}
