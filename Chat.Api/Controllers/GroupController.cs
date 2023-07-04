using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Authorize(Policy = "AllUsers")]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IValidator<CreateGroupRequest> _newGroupValidator;

        public GroupController(IGroupService groupService, IValidator<CreateGroupRequest> validator)
        {
            _groupService = groupService;
            _newGroupValidator = validator;
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

        [HttpGet("info/{groupId}")]
        public async Task<IActionResult> GetGroupById(string groupId)
        {
            var group = await _groupService.GetGroupViewByIdAsync(groupId);
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
            var validation = await _newGroupValidator.ValidateAsync(groupRequest);
            if (!validation.IsValid)
                return BadRequest(new RequestResult { Success = false, Errors = validation.Errors.Select(e => e.ErrorMessage) });

            await _groupService.CreateNewGroup(groupRequest);

            return Ok();
        }
    }
}
