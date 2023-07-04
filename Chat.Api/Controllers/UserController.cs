using Chat.BLL.DTO;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatApp.API.Controllers
{
    [Authorize(Policy = "AllUsers")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserDTO> _userDtoValidator;

        public UserController(IUserService userService, IValidator<UserDTO> userDtoValidator)
        {
            _userService = userService;
            _userDtoValidator = userDtoValidator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers([FromQuery] SearchParameters searchParameters)
        {
            var users = await _userService.GetAllUsersAsync(searchParameters);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("all/emails/{makerEmail}")]
        public async Task<IActionResult> GetEmailsExceptMakerAsync(string makerEmail)
        {
            var emails = await _userService.GetEmailsExceptMakerAsync(makerEmail);
            var result = JsonConvert.SerializeObject(emails);
            
            return Ok(result);
        }

        [HttpGet("allinfo/except/{makerEmail}")]
        public async Task<IActionResult> GetUsersShortInfoExceptMakerAsync(string? makerEmail)
        {
            var users = await _userService.GetUsersShortInfoExceptMakerAsync(makerEmail);
            return Ok(users);
        }

        [HttpGet("id/{userId}")]
        public async Task<IActionResult> GetUserById( string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromForm] UserUpdateRequest userUpdateRequest)
        {
            if (userUpdateRequest.UserDTO == null)
                return BadRequest();

            var userValidation = await _userDtoValidator.ValidateAsync(userUpdateRequest.UserDTO);
            if (!userValidation.IsValid)
                return StatusCode(400, userValidation.Errors.Select(e => e.ErrorMessage));

            await _userService.UpdateUserInfoAsync(userUpdateRequest);
            return Ok();
        }
    }
}
