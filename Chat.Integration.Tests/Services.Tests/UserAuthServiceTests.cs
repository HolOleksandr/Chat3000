using Chat.BLL.Exceptions;
using Chat.BLL.Models;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Integration.Tests.Services.Tests
{
    [TestFixture]
    public class UserAuthServiceTests
    {
        private IUserAuthService _userAuthService;

        [SetUp]
        public void Setup()
        {
            var helper = new TestHelpers();
            _userAuthService = helper.GetUserAuthService();
            TestHelpers.ResetUserTable();
        }

        [TestCase("admin@example.com", "Qwe123++", true)]
        [TestCase("admin@example.com", "Qwe12333++", false)]
        [TestCase("notregistered@example.com", "Qwe123++", false)]
        public async Task ValidateUserAsync_ReturnsResults(string email, string password, bool isValid)
        {
            // arrange
            var model = new UserLoginModel() { Email = email, Password = password };
            // act
            var result = await _userAuthService.ValidateUserAsync(model);
            // assert
            Assert.That(result, Is.EqualTo(isValid));
        }

        [TestCase("admin@example.com", "Qwe123++")]
        public async Task CreateTokenAsync_ReturnsToken(string email, string password)
        {
            // Arrange
            var model = new UserLoginModel() { Email = email, Password = password };

            // act
            var authResult = await _userAuthService.ValidateUserAsync(model);
            var token = await _userAuthService.CreateTokenAsync();
            var tokenParts = token.Trim().Split('.').Count();

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(token, Is.Not.Empty);
                Assert.That(tokenParts, Is.EqualTo(3));
            });
        }

        [Test]
        public async Task RegisterUserAsync_ReturnsSuccessResult_WhenUserCreated()
        {
            // arrange
            var userModel = new UserRegistrationModel()
            {
                Email = "testNEWuser@example.com",
                Password = "Qwe123++",
                FirstName = "Test",
                LastName = "User"
            };
            var model = new UserLoginModel() { Email = userModel.Email, Password = userModel.Password };

            // act
            var isAbleToLoginBefore = await _userAuthService.ValidateUserAsync(model);
            var result = await _userAuthService.RegisterUserAsync(userModel);
            var isAbleToLoginAfter = await _userAuthService.ValidateUserAsync(model);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Succeeded);
                Assert.That(isAbleToLoginBefore, Is.False);
                Assert.That(isAbleToLoginAfter, Is.True);
            });
        }

        [Test]
        public void RegisterUserAsync_ThrowsException_WhenPasswordIsNull()
        {
            // arrange
            var userModel = new UserRegistrationModel()
            {
                Email = "testNEWuser@example.com",
                Password = null,
                FirstName = "Test",
                LastName = "User"
            };
            //act
            async Task act() => await _userAuthService.RegisterUserAsync(userModel);
            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("Password is empty"));
        }

        [Test]
        public async Task ChangePasswordAsync_ReturnsResult_WhenChangePasswordSucceeds()
        {
            // arrange
            var model = new ChangePasswordModel
            { 
                Email = "admin@example.com", 
                OldPassword = "Qwe123++", 
                NewPassword = "Qwe123456+++",
                NewPasswordConfirm = "Qwe123456+++"
            };
            var OldLoginModel = new UserLoginModel() { Email = model.Email, Password = model.OldPassword };
            var NewLoginModel = new UserLoginModel() { Email = model.Email, Password = model.NewPassword };

            // act
            var isAbleToLoginBefore = await _userAuthService.ValidateUserAsync(OldLoginModel);
            var result = await _userAuthService.ChangePasswordAsync(model);
            var isAbleToLoginWithOldPass = await _userAuthService.ValidateUserAsync(OldLoginModel);
            var isAbleToLoginWithNewPass = await _userAuthService.ValidateUserAsync(NewLoginModel);
            // assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Succeeded);
                Assert.That(isAbleToLoginBefore, Is.True);
                Assert.That(isAbleToLoginWithOldPass, Is.False);
                Assert.That(isAbleToLoginWithNewPass, Is.True);
            });
        }

        [Test]
        public void ChangePasswordAsync_ReturnsException_WhenUserNotExist()
        {
            // arrange
            var model = new ChangePasswordModel
            {
                Email = "notRegisteredUser@example.com",
                OldPassword = "Qwe123++",
                NewPassword = "Qwe123456+++",
                NewPasswordConfirm = "Qwe123456+++"
            };
            //act
            async Task act() => await _userAuthService.ChangePasswordAsync(model);
            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("User is not exist"));
        }

        [Test]
        public async Task ChangePasswordAsync_ReturnsResultFalse_WhenChangePasswordNotSucceeds()
        {
            //arrange
            var model = new ChangePasswordModel
            {
                Email = "admin@example.com",
                OldPassword = "wrongPass123++",
                NewPassword = "Qwe123456+++",
                NewPasswordConfirm = "Qwe123456+++"
            };

            // act
            var result = await _userAuthService.ChangePasswordAsync(model);
            
            // assert
            Assert.That(result.Succeeded, Is.False);
        }
    }
}
