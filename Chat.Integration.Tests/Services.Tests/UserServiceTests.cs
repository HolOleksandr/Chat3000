using Chat.BLL.DTO;
using Chat.BLL.Exceptions;
using Chat.BLL.Models.Paging;
using Chat.BLL.Services.Implementation;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Implementation;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Implementation;
using Chat.DAL.UoW.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Integration.Tests.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _userService;
        
        [SetUp]
        public void Setup()
        {
            var helper = new TestHelpers();
            _userService = helper.GetUserService();
            TestHelpers.ResetUserTable();
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            //arrange
            var searchParams = new SearchParameters() {PageIndex = 0, PageSize = 5 };
            //act
            var users = await _userService.GetAllUsersAsync(searchParams);
            //assert
            Assert.That(users.Data, Has.Count.EqualTo(3));
        }

        [Test]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            //arrange
            var userModel = new User() { Id = "57f9b20f-3d14-4f48-be5f-90084218b437", Email = "admin@example.com" };
            //act
            var user = await _userService.GetUserByIdAsync(userModel.Id);
            //assert
            Assert.That(user.Email, Is.EqualTo(userModel.Email));
        }

        [Test]
        public async Task GetUserByIdAsync_ReturnsNull_IfUserNotExist()
        {
            //arrange
            var userModel = new User() { Id = "some-wrong-id", Email = "admin@example.com" };
            //act
            var user = await _userService.GetUserByIdAsync(userModel.Id);
            //assert
            Assert.That(user, Is.Null);
        }

        [Test]
        public async Task GetEmailsExceptMakerAsync_ReturnsEmailsExceptCaller()
        {
            //arrange
            var callerEmail = "admin@example.com";
            var emails = new string[] { "test22@example.com", "test2@example.com" };
            //act
            var emailsList = await _userService.GetEmailsExceptMakerAsync(callerEmail);
            
            //assert
            Assert.Multiple(() =>
            {
                Assert.That(emailsList.Count, Is.EqualTo(2));
                Assert.That(emailsList, Does.Contain(emails[0]));
                Assert.That(emailsList, Does.Contain(emails[1]));
            });
        }

        [Test]
        public async Task GetUsersShortInfoExceptMakerAsync_ReturnsUsersExceptCaller()
        {
            //arrange
            var callerEmail = "admin@example.com";
            var emailsList = new string[] { "test22@example.com", "test2@example.com" };
            var allUsers = DataSeed.GetfakeUserList();
            var firstUser = allUsers.FirstOrDefault(u => u.Email == emailsList[0]);
            var secondUser = allUsers.FirstOrDefault(u => u.Email == emailsList[1]);

            //act
            var usersList = await _userService.GetUsersShortInfoExceptMakerAsync(callerEmail);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(usersList.Count, Is.EqualTo(2));
                Assert.That(usersList.FirstOrDefault(u => u.Email == firstUser.Email
                    && u.Id == firstUser.Id), Is.Not.Null);
                Assert.That(usersList.FirstOrDefault(u => u.Email == secondUser.Email
                    && u.Id == secondUser.Id), Is.Not.Null);
            });
        }

        [Test]
        public async Task UpdateUserInfoAsync_UpdateInfoSuccessfully()
        {
            //arrange
            var newUserInfo = new UserDTO() {
                Id = "57f9b20f-3d14-4f48-be5f-90084218b437",
                Email = "adminNew@example.com",
                FirstName = "NewAdminName",
                LastName = "NewAdmin",
            };

            //act
            await _userService.UpdateUserInfoAsync(newUserInfo);
            var updatedUser = await _userService.GetUserByIdAsync(newUserInfo.Id);

            //assert
            Assert.Multiple(() =>
            {
                Assert.That(updatedUser.FirstName, Is.EqualTo(newUserInfo.FirstName));
                Assert.That(updatedUser.Email, Is.EqualTo(newUserInfo.Email));
                Assert.That(updatedUser.LastName, Is.EqualTo(newUserInfo.LastName));
            });
        }

        [Test]
        public void UpdateUserInfoAsync_ThrowException_WithWrongUserId()
        {
            //arrange
            var newUserInfo = new UserDTO()
            {
                Id = "incorrect-user-id",
                Email = "admin@example.com",
                FirstName = "NewAdminName",
                LastName = "NewAdmin",
            };

            //act
            async Task act() => await _userService.UpdateUserInfoAsync(newUserInfo);

            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("User is not exist"));
        }

        [Test]
        public void UpdateUserInfoAsync_ThrowException_IfEmailAlreadyExist()
        {
            //arrange
            var newUserInfo = new UserDTO()
            {
                Id = "57f9b20f-3d14-4f48-be5f-90084218b437",
                Email = "test22@example.com", // Already registered user's email
                FirstName = "NewAdminName",
                LastName = "NewAdmin",
            };

            //act
            async Task act() => await _userService.UpdateUserInfoAsync(newUserInfo);

            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("An account with this email address already exists"));
        }
        
    }
}
