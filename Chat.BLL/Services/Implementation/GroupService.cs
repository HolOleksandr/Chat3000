using AutoMapper;
using AutoMapper.Execution;
using Chat.BLL.DTO;
using Chat.BLL.Exceptions;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chat.BLL.Services.Implementation
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

        public async Task CreateNewGroup(CreateGroupRequest groupRequest)
        {
            var _newGroup = _mapper.Map<DAL.Entities.Group>(groupRequest);
            _newGroup.CreationDate = DateTime.Now;
            await _unitOfWork.GetRepository<IGroupRepository>().AddAsync(_newGroup);
            await _unitOfWork.SaveAsync();

            foreach (var member in groupRequest.Members)
            {
                if (member != null)
                {
                    await AddNewUserGroup(_newGroup.Id, member.Id!);
                }
            }
            await AddNewUserGroup(_newGroup.Id, _newGroup.AdminId);
            await _unitOfWork.SaveAsync();
        }

        private async Task AddNewUserGroup(int groupId, string userId) 
        {
            var _userGroupRepo = _unitOfWork.GetRepository<IUserGroupRepository>();
            var newUserGroup = new UserGroup()
            {
                GroupId = groupId,
                UserId = userId,
                JoinDate = DateTime.Now
            };
            await _userGroupRepo.AddAsync(newUserGroup);
        }

        public async Task<IEnumerable<GroupDTO>> GetGroupsAsync()
        {
            var groups = await _unitOfWork.GetRepository<IGroupRepository>().GetAllGroupsWithUsersAsync();
            var mappedGroups = _mapper.Map<IEnumerable<GroupDTO>>(groups);
            return mappedGroups;
        }

        public async Task<FilterResult<GroupInfoViewDTO>> GetGroupsByUserEmailAsync(string userEmail, SearchParameters searchParameters)
        {
            var _groupRepo = _unitOfWork.GetRepository<IGroupRepository>();
            
            var groups = await _groupRepo.GetGroupsByUserEmailAsync(userEmail, searchParameters.FilterQuery) 
                ?? throw new ChatException("User was not found.");

            var mappedGroups = _mapper.Map<IEnumerable<GroupInfoViewDTO>>(groups);
                
            var result = await FilterResult<GroupInfoViewDTO>.CreateAsync(
                mappedGroups,
                searchParameters.PageIndex,
                searchParameters.PageSize,
                searchParameters.SortColumn,
                searchParameters.SortOrder,
                searchParameters.FilterQuery);
            return result;
        }
    }
}
