using Chat.BLL.DTO;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Integration.Tests.Services.Tests
{
    [TestFixture]
    public class GroupServiceTests
    {
        private IGroupService _groupService;

        [SetUp]
        public void Setup()
        {
            var helper = new TestHelpers();
            _groupService = helper.GetGroupService();
            TestHelpers.ResetUserTable();
            TestHelpers.ResetUserGroupTable();
            TestHelpers.ResetGroupTable();
            TestHelpers.ResetMessageTable();
        }

        [Test]
        public async Task CreateNewGroup_CreatesNewGroup()
        {
            //arrange
            var groupRequest = new CreateGroupRequest() 
            {
                Name = "NewGroup",
                Description = "test description",
                AdminId = "57f9b20f-3d14-4f48-be5f-90084218b437",
                Members = new List<UserShortInfoDTO>() 
                {
                    new UserShortInfoDTO() {Id = "361ca39f-a39e-46b1-921c-a38a9803fceb"},
                    new UserShortInfoDTO() {Id = "772aef6c-24fa-41f9-af4d-7aa907eb2356"}
                }
            };

            //act
            var groupsCount = (await _groupService.GetGroupsAsync()).Count();
            await _groupService.CreateNewGroup(groupRequest);
            var groupsCountWithNew = (await _groupService.GetGroupsAsync()).Count();

            //assert
            var expected = groupsCount + 1;
            Assert.That(groupsCountWithNew, Is.EqualTo(expected));
        }

    }
}
