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

        public async Task CreateNewChat(CreateGroupRequest groupRequest)
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

        public async Task<FilterResult<GroupDTO>> GetGroupsByUserEmailAsync(string userEmail, SearchParameters searchParameters)
        {
            FilterResult<GroupDTO> result;
            var user = await _unitOfWork.GetRepository<IUserRepository>().GetUserByEmailAsync(userEmail) 
                ?? throw new ChatException("User was not found.");

            var _groupRepo = _unitOfWork.GetRepository<IGroupRepository>();
            
            if (IsSortColumnIsCustomColumn(searchParameters.SortColumn, "members"))
            {
                result = await GetGroupsSortedByUsersQtyAsync(user.Id, searchParameters);
            }
            else if (IsSortColumnIsCustomColumn(searchParameters.SortColumn, "admin"))
            {
                result = await GetGroupsSortedByAdminEmailAsync(user.Id, searchParameters);
            }
            else
            {
                var groups = await _groupRepo.GetGroupsByUserIdAsync(user.Id, searchParameters.FilterQuery);
                var mappedGroups = _mapper.Map<IEnumerable<GroupDTO>>(groups);
                result = await FilterResult<GroupDTO>.CreateAsync(
                mappedGroups,
                searchParameters.PageIndex,
                searchParameters.PageSize,
                searchParameters.SortColumn,
                searchParameters.SortOrder,
                searchParameters.FilterQuery);

            }
            
            return result;
        }




        private async Task<FilterResult<GroupDTO>> GetGroupsSortedByUsersQtyAsync(string userId, SearchParameters searchParameters)
        {
            var ascOrder = IsAscOrder(searchParameters.SortOrder);

            var sortedGroups = await _unitOfWork.GetRepository<IGroupRepository>()
                .GetGroupsSortedByUsersQtyAsync(userId, ascOrder, searchParameters.FilterQuery);

            var result = await CreateCustomFilterResult(sortedGroups, searchParameters);
            return result;
        }




        private async Task<FilterResult<GroupDTO>> GetGroupsSortedByAdminEmailAsync(string userId, SearchParameters searchParameters)
        {
            var ascOrder = IsAscOrder(searchParameters.SortOrder);

            var sortedGroups = await _unitOfWork.GetRepository<IGroupRepository>()
                .GetGroupsSortedByAdminEmailAsync(userId, ascOrder,searchParameters.FilterQuery);

            var result = await CreateCustomFilterResult(sortedGroups, searchParameters);

            return result;
        }




        private async Task<FilterResult<GroupDTO>> CreateCustomFilterResult(IEnumerable<DAL.Entities.Group> groups, SearchParameters parameters)
        {
            var count = groups.Count();
            var pagedData = groups.Skip((parameters.PageIndex) * parameters.PageSize).Take(parameters.PageSize);
            var mappedSortedGroups = _mapper.Map<IEnumerable<GroupDTO>>(pagedData);
            var result = await FilterResult<GroupDTO>.CreateAsync(mappedSortedGroups, 0, 0, null, null);
           
            result.Data = mappedSortedGroups.ToList();
            result.FilterQuery = parameters.FilterQuery;
            result.PageIndex = parameters.PageIndex;
            result.PageSize = parameters.PageSize;
            result.TotalPages = (int)Math.Ceiling(count / (double)parameters.PageSize);
            result.TotalCount = count;

            return result;
        }

        private static bool IsSortColumnIsCustomColumn(string columnName, string customColumn)
        {
            if (string.IsNullOrEmpty(columnName)) return false;

            return string.Equals(columnName, customColumn, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsAscOrder(string order)
        {
            if (!string.Equals(order, "asc"))
            {
                return false;
            }
            return true;
        }

        


}
}
