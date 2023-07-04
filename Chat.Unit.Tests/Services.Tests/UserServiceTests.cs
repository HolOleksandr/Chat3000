using Chat.BLL.DTO;
using Chat.BLL.Exceptions;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Implementation;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace Chat.Tests.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IUserService _userService;
        private Mock<IBlobManager> _mockBlobManager;

        [SetUp]
        public void Setup()
        {
            _mockBlobManager = new Mock<IBlobManager>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _userService = new UserService(_mockUnitOfWork.Object, _mockBlobManager.Object, TestHelper.CreateMapperProfile());
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>().GetAllUsersWithFilterAsync(It.IsAny<string>()));
            var searchParams = new SearchParameters();
            //act
            await _userService.GetAllUsersAsync(searchParams);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IUserRepository>()
                .GetAllUsersWithFilterAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task UpdateUserInfoAsync_UpdatesUserInfo_WithSameEmail()
        {
            //arrange
            var user = new User() { Id = "id", Email = "test@example.com" };
            var userDTO = new UserDTO() { Id = "id", Email = "test@example.com" };
            var userUpdateRequest = new UserUpdateRequest() { UserDTO = userDTO, File = null };

            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetAllUsersWithFilterAsync(It.IsAny<string>()));
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetUserByStringIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            //act
            await _userService.UpdateUserInfoAsync(userUpdateRequest);
            //assert
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateUserInfoAsync_UpdatesUserInfo_WithFileUploading()
        {
            //arrange
            var user = new User() { Id = "id", Email = "test@example.com", Avatar = "avatarUrl" };
            var userDTO = new UserDTO() { Id = "id", Email = "test@example.com", Avatar = "avatarUrl" };
            var bytes = Encoding.UTF8.GetBytes("TestFile");
            var avatar = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "avatar.jpeg");
            var userUpdateRequest = new UserUpdateRequest() { UserDTO = userDTO, File = avatar };
            var newUrl = "newAvatarUrl";

            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetAllUsersWithFilterAsync(It.IsAny<string>()));
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetUserByStringIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockBlobManager.Setup(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), null)).ReturnsAsync(newUrl);

            //act
            await _userService.UpdateUserInfoAsync(userUpdateRequest);
            //assert
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
            _mockBlobManager.Verify(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), null), Times.Once);
        }

        [Test]
        public void UpdateUserInfoAsync_ThrowException_UserIsNotExist()
        {
            //arrange
            var userDTO = new UserDTO() { Id = "id", Email = "test@example.com" };
            var userUpdateRequest = new UserUpdateRequest() { UserDTO = userDTO, File = null };

            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetUserByStringIdAsync(It.IsAny<string>())).ReturnsAsync((string userId) => null);
            //act
            async Task act() => await _userService.UpdateUserInfoAsync(userUpdateRequest);

            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("User is not exist"));
        }

        [Test]
        public async Task UpdateUserInfoAsync_WithAnotherEmail_UpdatesUserInfo()
        {
            //arrange
            var user = new User() { Id = "id", Email = "test@example.com" };
            var userDTO = new UserDTO() { Id = "id", Email = "newEmail@example.com" };
            var userUpdateRequest = new UserUpdateRequest() { UserDTO = userDTO, File = null };

            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetAllUsersWithFilterAsync(It.IsAny<string>()));
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetUserByStringIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .IsEmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            //act
            await _userService.UpdateUserInfoAsync(userUpdateRequest);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IUserRepository>()
                .IsEmailExistsAsync(It.IsAny<string>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public void UpdateUserInfoAsync_ThrowException_IfNewEmailIsAlreadyExist()
        {
            //arrange
            var user = new User() { Id = "id", Email = "test@example.com" };
            var userDTO = new UserDTO() { Id = "id", Email = "newEmail@example.com" };
            var userUpdateRequest = new UserUpdateRequest() { UserDTO = userDTO, File = null };

            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetAllUsersWithFilterAsync(It.IsAny<string>()));
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetUserByStringIdAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .IsEmailExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            //act
            async Task act() => await _userService.UpdateUserInfoAsync(userUpdateRequest);
            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("An account with this email address already exists"));
        }

        [Test]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>().GetUserByStringIdAsync(It.IsAny<string>()));
            string userId = "id";
            //act
            await _userService.GetUserByIdAsync(userId);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IUserRepository>()
                .GetUserByStringIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetUserByIdAsync_ReturnsNull_IfUserNotExist()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>()
                .GetUserByStringIdAsync(It.IsAny<string>())).ReturnsAsync((string userId) => null);
            string userId = "id";
            //act
            var user = await _userService.GetUserByIdAsync(userId);
            //assert
            Assert.That(user, Is.Null);
        }

        [Test]
        public async Task GetEmailsExceptMakerAsync_ReturnsEmails()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>().GetEmailsListExceptMakerAsync(It.IsAny<string>()));
            string userEmail = "email";
            //act
            await _userService.GetEmailsExceptMakerAsync(userEmail);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IUserRepository>()
                .GetEmailsListExceptMakerAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetUsersShortInfoExceptMakerAsync_ReturnsUsers()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserRepository>().GetAllUsersExceptMakerAsync(It.IsAny<string>()));
            string userEmail = "email";
            //act
            await _userService.GetUsersShortInfoExceptMakerAsync(userEmail);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IUserRepository>()
                .GetAllUsersExceptMakerAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
