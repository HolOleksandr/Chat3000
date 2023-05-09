using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Implementation;
using Chat.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Tests.Repository.Tests
{
    [TestFixture]
    public class GroupRepositoryTests
    {
        private static GroupRepository GetGroupRepository()
        {
            var context = new ChatDbContext(TestHelper.GetUnitTestDbOptions());
            GroupRepository groupRepository = new(context);
            return groupRepository;
        }

        [Test]
        public async Task AddAsync_AddsNewGroupValue()
        {
            //Arrange
            var context = new ChatDbContext(TestHelper.GetUnitTestDbOptions());
            GroupRepository groupRepository = new(context);
            var newGroup = new Group
            {
                Name = "New Group",
                AdminId = "57f9b20f-3d14-4f48-be5f-90084218b437",
                CreationDate = DateTime.Parse("2023-04-24"),
                Description = "New Group group description",
            };

            //Act
            await groupRepository.AddAsync(newGroup);
            context.SaveChanges();
            var groups = await groupRepository.GetAllAsync();

            //Assert
            Assert.That(groups, Is.Not.Null);
            Assert.That(groups.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task GetAllGroupsWithUsersAsync_ReturnsAllGroups()
        {
            //Arrange
            var _groupRepository = GetGroupRepository();

            //Act
            var groups = await _groupRepository.GetAllAsync();

            //Assert
            Assert.That(groups, Is.Not.Null);
            Assert.That(groups.Count(), Is.EqualTo(3));
        }

        [TestCase("admin@example.com", 3)]
        [TestCase("test2@example.com", 2)]
        public async Task GetGroupsByUserEmailAsync_ReturnsUserGroups(string userId, int groupsCount)
        {
            //Arrange
            var _groupRepository = GetGroupRepository();

            //Act
            var groups = await _groupRepository.GetGroupsByUserEmailAsync(userId, "");

            //Assert
            Assert.That(groups, Is.Not.Null);
            Assert.That(groups.Count(), Is.EqualTo(groupsCount));
        }

        [Test]
        public async Task GetAllGroupsWithUsersAsync_ReturnsGroupsWithUsers()
        {
            //Arrange
            var _groupRepository = GetGroupRepository();

            //Act
            var groups = await _groupRepository.GetAllGroupsWithUsersAsync();

            //Assert
            Assert.That(groups, Is.Not.Null);
            Assert.That(groups.SelectMany(x => x.Users).Count(), Is.EqualTo(7));
        }


    }
}
