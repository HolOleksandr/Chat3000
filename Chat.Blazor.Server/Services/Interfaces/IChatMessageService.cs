using Chat.Blazor.Server.Models.DTO;
using Chat.Blazor.Server.Models.Responses;

namespace Chat.Blazor.Server.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<IEnumerable<MessageDTO>> GetAllMessagesInGroupAsync(int groupId);
        Task<RequestResult> SendMessageAsync(MessageDTO newMessage);
    }
}
