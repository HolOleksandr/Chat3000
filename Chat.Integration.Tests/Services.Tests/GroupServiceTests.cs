using Chat.BLL.DTO;
using Chat.BLL.Exceptions;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Interfaces;

namespace Chat.Integration.Tests.Services.Tests
{
    [TestFixture]
    public class GroupServiceTests
    {
        private IGroupService _groupService;
        TestHelpers _testHelpers;

        [SetUp]
        public void Setup()
        {
            _testHelpers = new TestHelpers();
            _groupService = _testHelpers.GetGroupService();
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
            var groupsCount = (await _groupService.GetGroupsAsync()).Count();

            //act
            await _groupService.CreateNewGroup(groupRequest);
            var allGroups = await _groupService.GetGroupsAsync();
            var groupsCountWithNew = allGroups.Count();
            var addedNewGroup = allGroups.FirstOrDefault(g => g.Name == groupRequest.Name);

            //assert
            var expected = groupsCountWithNew;
            Assert.Multiple(() =>
            {
                Assert.That(groupsCountWithNew, Is.EqualTo(expected));
                Assert.That(addedNewGroup.AdminId, Is.EqualTo(groupRequest.AdminId));
                Assert.That(addedNewGroup.Description, Is.EqualTo(groupRequest.Description));
                Assert.That(addedNewGroup.Name, Is.EqualTo(groupRequest.Name));
            });
        }

        [Test]
        public async Task GetGroupsAsync_ReturnsGroups()
        {
            //arrange
            var groups = DataSeed.GetfakeGroupList();
            var expectedCount = groups.Count();

            //act
            var actualGroups = await _groupService.GetGroupsAsync();
            var actualCount = actualGroups.Count();

            //assert
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [Test]
        public async Task GetGroupsByUserEmailAsync_ReturnsUserGroups()
        {
            //arrange
            var userEmail = "admin@example.com";
            var users = DataSeed.GetfakeUserList();
            var user = users.FirstOrDefault(u => u.Email == userEmail);
            var userGroups = DataSeed.GetfakeUserGroupList();
            var expectedCount = userGroups.Where(u => u.UserId == user.Id).Count();
            var searchParams = new SearchParameters() { PageIndex=0, PageSize=100 };

            //act
            var actualGroups = await _groupService.GetGroupsByUserEmailAsync(userEmail, searchParams);
            var actualCount = actualGroups.Data.Count;

            //assert
            Assert.That(actualCount, Is.EqualTo(expectedCount));
        }

        [Test]
        public void GetGroupsByUserEmailAsync_TrowsException_IfUserNotExist()
        {
            //arrange
            var userEmail = "notExpected@example.com";
            var searchParams = new SearchParameters() { PageIndex = 0, PageSize = 100 };

            //act
            async Task act() => await _groupService.GetGroupsByUserEmailAsync(userEmail, searchParams);

            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("User was not found."));
        }

    }
}
