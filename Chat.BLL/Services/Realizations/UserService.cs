using AutoMapper;
using Chat.BLL.DTO;
using Chat.BLL.Exceptions;
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
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
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

        public async Task UpdateUserInfoAsync(UserDTO userUpdateModel)
        {
            
            var user = await _unitOfWork.GetRepository<IUserRepository>().GetUserByStringIdAsync(userUpdateModel.Id);

            if (user == null)
            {
                throw new ChatException("User is not exist");
            }

            if (!string.Equals(user.Email, userUpdateModel.Email, StringComparison.OrdinalIgnoreCase))
            {
                await CheckUserEmailAsync(userUpdateModel.Email);
            }

            user.FirstName = userUpdateModel.FirstName;
            user.LastName = userUpdateModel.LastName;
            user.Email = userUpdateModel.Email;
            user.PhoneNumber = userUpdateModel.PhoneNumber;
            user.UserName = userUpdateModel.Email;
            user.Nickname = userUpdateModel.Nickname;
            user.Avatar = userUpdateModel.Avatar;

            await _unitOfWork.SaveAsync();

        }

        private async Task CheckUserEmailAsync(string email)
        {

            var isExist = await _unitOfWork.GetRepository<IUserRepository>().IsEmailExistsAsync(email);
            if (isExist)
            {
                throw new ChatException("An account with this email address already exists");
            }

        }

    }

   
}
