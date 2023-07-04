using Chat.Blazor.WebAssembly.Models.DTO;
using Chat.Blazor.WebAssembly.Models.Responses;

namespace Chat.Blazor.WebAssembly.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<IEnumerable<MessageDTO>> GetAllMessagesInGroupAsync(int groupId);
        Task<RequestResult> SendMessageAsync(MessageDTO newMessage);
    }
}