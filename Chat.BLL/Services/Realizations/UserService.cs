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
            user.BirthDate = userUpdateModel.BirthDate;

            await _unitOfWork.SaveAsync();

        }

        public async Task<UserDTO?> GetUserByIdAsync(string userId)
        {
            var user = await _unitOfWork.GetRepository<IUserRepository>().GetUserByStringIdAsync(userId);
            
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<string>> GetEmailsExceptMakerAsync (string makerEmail)
        {
            var emails = await _unitOfWork.GetRepository<IUserRepository>().GetEmailsListExceptMakerAsync(makerEmail);
            return emails;
        }

        public async Task<IEnumerable<UserShortInfoDTO>> GetUsersShortInfoExceptMakerAsync(string? makerEmail)
        {
            var usersInfo = await _unitOfWork.GetRepository<IUserRepository>().GetAllUsersExceptMakerAsync(makerEmail); 

            var mappedInfo = _mapper.Map<IEnumerable<UserShortInfoDTO>>(usersInfo);
            return mappedInfo;
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
