using Chat.BLL.DTO;

namespace Chat.BLL.Services.Interfaces
{
    public interface IMessageService
    {
        Task AddNewMessageAsync(MessageDTO message);
        Task<IEnumerable<MessageDTO>> GetAllMessagesByGroupIdasync(int groupId);
    }
}
