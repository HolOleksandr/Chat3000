using Chat.BLL.DTO;
using Chat.BLL.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Authorize(Policy = "AllUsers")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IValidator<MessageDTO> _messageValidator;
        public MessageController(IMessageService messageService, IValidator<MessageDTO> messageValidator)
        {
            _messageService = messageService;
            _messageValidator = messageValidator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewMessage([FromBody] MessageDTO message)
        {
            //var validation = await _messageValidator.ValidateAsync(message);
            //if (!validation.IsValid)
            //    return StatusCode(400, validation.Errors.Select(e => e.ErrorMessage));

            await _messageService.AddNewMessageAsync(message);
            return Ok();
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetAllMessagesByGroupId(int groupId)
        {
            var messages = await _messageService.GetAllMessagesByGroupIdasync(groupId);
            if (messages == null)
                return Ok("This group doesnt has messages yet");
            return Ok(messages);
        }

    }
}

