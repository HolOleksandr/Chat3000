using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Tests.Repository.Tests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            var context = new ChatDbContext(TestHelper.GetUnitTestDbOptions());
            _userRepository = new(context);
        }

        [TestCase("admin@example.com", 1)]
        [TestCase("User", 2)]
        [TestCase("notfoundUsers", 0)]
        public async Task GetAllUsersWithFilterAsync_ReturnsUsers(string? searchText, int usersCount)
        {
            //Arrange
            //Act
            var users = await _userRepository.GetAllUsersWithFilterAsync(searchText);
            //Assert
            Assert.That(users.Count(), Is.EqualTo(usersCount));
        }

        [TestCase("57f9b20f-3d14-4f48-be5f-90084218b437")]
        [TestCase("361ca39f-a39e-46b1-921c-a38a9803fceb")]
        public async Task GetUserByStringIdAsync_ReturnsUser(string? userId)
        {
            //Arrange
            //Act
            var user = await _userRepository.GetUserByStringIdAsync(userId);
            //Assert
            Assert.That(user.Id, Is.EqualTo(userId));
        }

        [TestCase("someStrangeUserId")]
        public async Task GetUserByStringIdAsync_ReturnsNull_IfIdNotExcept(string? userId)
        {
            //Arrange
            //Act
            var user = await _userRepository.GetUserByStringIdAsync(userId);
            //Assert
            Assert.That(user, Is.Null);
        }

        [TestCase("admin@example.com")]
        [TestCase("test2@example.com")]
        public async Task GetUserByEmailAsync_ReturnsUser(string? userEmail)
        {
            //Arrange
            //Act
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            //Assert
            Assert.That(user.Email, Is.EqualTo(userEmail));
        }

        [TestCase("notExist@email.com")]
        public async Task GetUserByEmailAsync_ReturnsNull_IfIdNotExcept(string? userEmail)
        {
            //Arrange
            //Act
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            //Assert
            Assert.That(user, Is.Null);
        }

        [TestCase("admin@example.com", true)]
        [TestCase("test2@example.com", true)]
        [TestCase("notExist@email.com", false)]
        public async Task IsEmailExistsAsync_ReturnsUser(string? userEmail, bool isExist)
        {
            //Arrange
            //Act
            var result = await _userRepository.IsEmailExistsAsync(userEmail);
            //Assert
            Assert.That(result, Is.EqualTo(isExist));
        }

        [TestCase("admin@example.com", 2)]
        [TestCase("test2@example.com", 2)]
        public async Task GetEmailsListExceptMakerAsync_ReturnsUsersEmailsExceptCaller(string? userEmail, int EmailsCount)
        {
            //Arrange
            //Act
            var emails = await _userRepository.GetEmailsListExceptMakerAsync(userEmail);
            var result = emails.ToList().Contains(userEmail);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(emails.Count(), Is.EqualTo(EmailsCount));
                Assert.That(result, Is.False);
            });
        }

        [TestCase("admin@example.com", 2)]
        public async Task GetAllUsersExceptMakerAsync_ReturnsUsersExceptCaller(string? userEmail, int usersCount)
        {
            //Arrange
            //Act
            var users = await _userRepository.GetAllUsersExceptMakerAsync(userEmail);
            var user = users.FirstOrDefault(u => u.Email == userEmail);
            var result = true;
            if (user == null)
            {
                result = false;
            }
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(users.Count(), Is.EqualTo(usersCount));
                Assert.That(result, Is.False);
            });
        }
    }
}
