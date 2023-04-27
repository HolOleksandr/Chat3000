using Chat.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Services.Interfaces
{
    public interface IMessageService
    {
        Task AddNewMessageAsync(MessageDTO message);
        Task<IEnumerable<MessageDTO>> GetAllMessagesByGroupIdasync(int groupId);
    }
}
