using AutoMapper;
using Chat.BLL.Exceptions;
using Chat.BLL.Models;
using Chat.BLL.Services.Implementation;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Tests.Services.Tests
{
    [TestFixture]
    public class UserAuthServiceTests
    {
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IMapper> _mockMapper;
        private IUserAuthService _userAuthService;

        [SetUp]
        public void Setup()
        {
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _mockConfiguration = new Mock<IConfiguration>();
            _mockMapper = new Mock<IMapper>();
            _userAuthService = new UserAuthService(_mockUserManager.Object, _mockConfiguration.Object, _mockMapper.Object);
        }
        [Test]
        public async Task CreateTokenAsync_ReturnsToken()
        {
            // Arrange
            var userModel = new UserLoginModel { Email = "test@example.com", Password = "password123" };
            var user = new User { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByNameAsync(userModel.Email))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, userModel.Password))
                .ReturnsAsync(true);
            await _userAuthService.ValidateUserAsync(userModel);
            var mockClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "User"),
            };
            var roles = new List<string> { "Role1", "Role2" };
            _mockUserManager.Setup(x => x.GetClaimsAsync(It.IsAny<User>())).ReturnsAsync(mockClaims);
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns("test_key_Long123");
            _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("test_issuer");

            // act
            var result = await _userAuthService.CreateTokenAsync();

            // assert
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task RegisterUserAsync_ReturnsSuccessResult_WhenUserCreated()
        {
            // arrange
            var userModel = new UserRegistrationModel()
            {
                Email = "testuser@example.com",
                Password = "testpassword",
                FirstName = "Test",
                LastName = "User"
            };
            var user = new User()
            {
                Id = "1",
                Email = userModel.Email,
                UserName = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };
            _mockMapper.Setup(x => x.Map<User>(It.IsAny<UserRegistrationModel>())).Returns(user);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // act
            var result = await _userAuthService.RegisterUserAsync(userModel);

            // assert
            Assert.That(result.Succeeded);
        }

        [Test]
        public void RegisterUserAsync_ThrowsException_WhenPasswordIsNull()
        {
            // Arrange
            var userModel = new UserRegistrationModel() { Email = "testuser@example.com" };
            var user = new User() { Id = "1", Email = userModel.Email, UserName = userModel.Email };
            _mockMapper.Setup(x => x.Map<User>(It.IsAny<UserRegistrationModel>())).Returns(user);

            //act
            async Task act() => await _userAuthService.RegisterUserAsync(userModel);
            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("Password is empty"));
        }

        [Test]
        public async Task ValidateUserAsync_ReturnsTrue_WhenLoginDatalCorrect()
        {
            // Arrange
            var userModel = new UserLoginModel { Email = "test@example.com", Password = "password123" };
            var user = new User { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByNameAsync(userModel.Email))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, userModel.Password))
                .ReturnsAsync(true);
            // act
            var result = await _userAuthService.ValidateUserAsync(userModel);
            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ValidateUserAsync_ReturnsFalse_WhenLoginDatalIncorrect()
        {
            // arrange
            var userModel = new UserLoginModel { Email = "test@example.com", Password = "password123" };
            var user = new User { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByNameAsync(userModel.Email))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, userModel.Password))
                .ReturnsAsync(false);
            // act
            var result = await _userAuthService.ValidateUserAsync(userModel);
            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ValidateUserAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            // arrange
            var userModel = new UserLoginModel { Email = "test@example.com", Password = "password123" };
            _mockUserManager.Setup(x => x.FindByNameAsync(userModel.Email))
                .ReturnsAsync((string email) => null);
            // act
            var result = await _userAuthService.ValidateUserAsync(userModel);
            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ChangePasswordAsync_ReturnsResult_WhenChangePasswordSucceeds()
        {
            // arrange
            var model = new ChangePasswordModel { Email = "test@example.com", OldPassword = "oldpassword", NewPassword = "newpassword" };
            var user = new User { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByEmailAsync(model.Email))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.ChangePasswordAsync(user, model.OldPassword, model.NewPassword))
                .ReturnsAsync(IdentityResult.Success);
            // act
            var result = await _userAuthService.ChangePasswordAsync(model);
            // assert
            Assert.That(result.Succeeded, Is.True);
        }

        [Test]
        public void ChangePasswordAsync_ReturnsException_WhenUserNotExist()
        {
            // arrange
            var model = new ChangePasswordModel { Email = "test@example.com", OldPassword = "oldpassword", NewPassword = "newpassword" };
            var user = new User { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByEmailAsync(model.Email))
                .ReturnsAsync((string email) => null);
            _mockUserManager.Setup(x => x.ChangePasswordAsync(user, model.OldPassword, model.NewPassword))
                .ReturnsAsync(IdentityResult.Success);
            //act
            async Task act() => await _userAuthService.ChangePasswordAsync(model);
            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("User is not exist"));
        }

        [Test]
        public async Task ChangePasswordAsync_ReturnsResultFalse_WhenChangePasswordNotSucceeds()
        {
            // arrange
            var model = new ChangePasswordModel { Email = "test@example.com", OldPassword = "oldpassword", NewPassword = "newpassword" };
            var user = new User { Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByEmailAsync(model.Email))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.ChangePasswordAsync(user, model.OldPassword, model.NewPassword))
                .ReturnsAsync(IdentityResult.Failed());
            // act
            var result = await _userAuthService.ChangePasswordAsync(model);
            // assert
            Assert.That(result.Succeeded, Is.False);
        }
    }
}
