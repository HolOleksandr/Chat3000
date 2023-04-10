using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chat3000Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
    }
}
