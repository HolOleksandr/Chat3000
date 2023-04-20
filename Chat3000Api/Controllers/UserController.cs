using Chat.BLL.Models.Paging;
using Chat.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatApp.API.Controllers
{
    //[Authorize(Policy = "AllUsers")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
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


    
    }
}
