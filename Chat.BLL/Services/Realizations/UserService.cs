using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Helpers;
using Chat.BLL.Models.Paging;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat.BLL.Services.Realizations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FilterResult<UserDTO>> GetAllUsersAsync(SearchParameters searchParameters)
        {

            var users = await _unitOfWork.GetRepository<IUserRepository>()
                .GetAllUsersWithFilterAsync(searchParameters.FilterQuery);
            var mappedUsers = _mapper.Map<IEnumerable<UserDTO>>(users);


            return await FilterResult<UserDTO>.CreateAsync(
                 mappedUsers,
                 searchParameters.PageIndex,
                 searchParameters.PageSize,
                 searchParameters.SortColumn,
                 searchParameters.SortOrder,
                 searchParameters.FilterQuery
                 );
        }

    }

   
}
