using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDTO>> GetGroupsAsync();
        Task<FilterResult<GroupDTO>> GetGroupsByUserEmailAsync(string userEmail, SearchParameters searchParameters);

        Task CreateNewChat(CreateGroupRequest groupRequest);
    }
}
