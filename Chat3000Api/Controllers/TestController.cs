using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [Authorize(Policy = "AllUsers")]
        [HttpGet()]
        public async Task<IActionResult> DownloadAnyFile()
        {
            return Ok("This is controller for registered users");
        }
    }
}
