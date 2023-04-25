using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Services.Realizations
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateNewChat(IEnumerable<string> emails)
        {
            var _userRepo = _unitOfWork.GetRepository<IUserRepository>();
            //var _groupRepo = 
            foreach (var email in emails)
            {
                _userRepo.GetUserByEmailAsync(email);

            }
        }

        public async Task<IEnumerable<GroupDTO>> GetGroupsAsync()
        {
            var groups = await _unitOfWork.GetRepository<IGroupRepository>().GetAllGroupsWithUsersAsync();
            var mappedGroups = _mapper.Map<IEnumerable<GroupDTO>>(groups);
            return mappedGroups;
        }

        public async Task<FilterResult<GroupDTO>> GetGroupsByUserEmailAsync(string userEmail, SearchParameters searchParameters)
        {
            var user = await _unitOfWork.GetRepository<IUserRepository>().GetUserByEmailAsync(userEmail);

            var groups = await _unitOfWork.GetRepository<IGroupRepository>().GetGroupsByUserIdAsync(user.Id, searchParameters.FilterQuery);
            var mappedGroups = _mapper.Map<IEnumerable<GroupDTO>>(groups);
            
            return await FilterResult<GroupDTO>.CreateAsync(
                 mappedGroups,
                 searchParameters.PageIndex,
                 searchParameters.PageSize,
                 searchParameters.SortColumn,
                 searchParameters.SortOrder,
                 searchParameters.FilterQuery
                 );
        }



    }
}
