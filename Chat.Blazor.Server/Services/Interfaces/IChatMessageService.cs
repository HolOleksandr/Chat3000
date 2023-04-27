using Chat.Blazor.Server.Models;
using Chat.Blazor.Server.Models.DTO;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<IEnumerable<MessageDTO>> GetAllMessagesInGroupAsync(int groupId);
        Task<RequestResult> SendMessageAsync(MessageDTO newMessage);
    }
}
