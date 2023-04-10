using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chat3000Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
