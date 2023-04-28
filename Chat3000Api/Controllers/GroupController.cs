using Chat.BLL.Models;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
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

        [HttpGet("user/group/{groupId}")]
        public async Task<IActionResult> GetGroupById(int groupId)
        {
            string group = null;
            //var group = await _groupService.GetGroupByIdAsync(groupId);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
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
        public async Task<IActionResult> CreateNewChat([FromBody] CreateGroupRequest groupRequest)
        {
            if (string.IsNullOrEmpty(groupRequest.Name) 
                || !groupRequest.Members.Any() 
                || string.IsNullOrEmpty(groupRequest.AdminId)) 
            {
                return BadRequest("You can not create this group");
            }
            await _groupService.CreateNewGroup(groupRequest);

            return Ok();
        }
    }
}
