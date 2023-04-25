using Chat.BLL.Models.Paging;
using Chat.BLL.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{


    //[Authorize(Policy = "AllUsers")]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;


        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupService.GetGroupsAsync();
            if (groups == null)
            {
                return NotFound();
            }
            return Ok(groups);
        }

        [HttpGet("user/{userEmail}")]
        public async Task<IActionResult> GetUserGroups(string userEmail, [FromQuery] SearchParameters searchParameters)
        {
            var groups = await _groupService.GetGroupsByUserEmailAsync(userEmail, searchParameters);
            if (groups == null)
            {
                return NotFound();
            }
            return Ok(groups);
        }

        [HttpPost("new")]
        public async Task<IActionResult> UpdateUser(IEnumerable<string> emails)
        {
            if (emails == null || !emails.Any()) 
            {
                return BadRequest("Emails list is empty");
            }
            await _groupService.CreateNewChat(emails);

            return Ok();
        }
    }
}
