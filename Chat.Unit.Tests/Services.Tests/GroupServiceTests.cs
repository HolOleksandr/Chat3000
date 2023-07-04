using Chat.BLL.DTO;
using Chat.BLL.Exceptions;
using Chat.BLL.Models.Paging;
using Chat.BLL.Models.Requests;
using Chat.BLL.Services.Implementation;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Data;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Implementation;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Interface;
using Moq;

namespace Chat.Tests.Services.Tests
{
    [TestFixture]
    public class GroupServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IGroupService _groupService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            var context = new ChatDbContext(TestHelper.GetUnitTestDbOptions());
            GroupRepository groupRepository = new(context);
            UserGroupRepository userGroupRepository = new(context);

            _mockUnitOfWork.Setup(x => x.GetRepository<IGroupRepository>())
                .Returns(groupRepository);
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserGroupRepository>())
                .Returns(userGroupRepository);
            _groupService = new GroupService(_mockUnitOfWork.Object, TestHelper.CreateMapperProfile());
        }

        [Test]
        public async Task GetGroupsAsync_ReturnsAllGroups()
        {
            //arrange
            //act
            var actual = await _groupService.GetGroupsAsync();
            //assert
            Assert.That(actual.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task CreateNewGroup_AddsNewGroup()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IGroupRepository>().AddAsync(It.IsAny<Group>()));
            _mockUnitOfWork.Setup(x => x.GetRepository<IUserGroupRepository>().AddAsync(It.IsAny<UserGroup>()));
            var _groupService = new GroupService(_mockUnitOfWork.Object, TestHelper.CreateMapperProfile());
            var newGroup = new CreateGroupRequest()
            {
                Name = "test",
                Description = "test",
                AdminId = "57f9b20f-3d14-4f48-be5f-90084218b437",
                Members = new List<UserShortInfoDTO>()
                {
                    new UserShortInfoDTO() {Id = "id1"},
                    new UserShortInfoDTO() {Id = "id2"}
                }
            };
            //act
            await _groupService.CreateNewGroup(newGroup);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IGroupRepository>()
                .AddAsync(It.Is<Group>(g => g.Name == newGroup.Name 
                    && g.AdminId == newGroup.AdminId 
                    && g.Description == newGroup.Description)), Times.Once);
            _mockUnitOfWork.Verify(x => x.GetRepository<IUserGroupRepository>()
                .AddAsync(It.IsAny<UserGroup>()), Times.Exactly(newGroup.Members.Count() + 1));
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Exactly(2));
        }

        [Test]
        public void GetGroupsByUserEmailAsync_ThrowsChatExceptionWithWrongUserEmail()
        {
            //arrange
            var userEmail = "wrong@email.com";
            var searchParameters = new SearchParameters();
            //act
            async Task act() => await _groupService.GetGroupsByUserEmailAsync(userEmail, searchParameters);
            //assert
            var ex = Assert.ThrowsAsync<ChatException>(() => act());
            Assert.That(ex.Message, Is.EqualTo("User was not found."));
        }

        [Test]
        public async Task Unit_GetGroupsByUserEmailAsync_ReturnsFilteredResult()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IGroupRepository>()
                .GetGroupsByUserEmailAsync(It.IsAny<string>(), It.IsAny<string>()));
            
            var _groupService = new GroupService(_mockUnitOfWork.Object, TestHelper.CreateMapperProfile());
            var userEmail = "email";
            var searchParameters = new SearchParameters();
            //act
            var result = await _groupService.GetGroupsByUserEmailAsync(userEmail, searchParameters);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IGroupRepository>()
                .GetGroupsByUserEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestCase("admin@example.com", 3)]
        [TestCase("test2@example.com", 2)]
        public async Task GetGroupsByUserEmailAsync_ReturnsFilteredResult(string userEmail, int groupsCount)
        {
            //arrange
            var searchParameters = new SearchParameters
            {
                PageIndex = 1,
                PageSize = 10,
                SortColumn = "Name",
                SortOrder = "asc",
                FilterQuery = ""
            };
            //act
            var result = await _groupService.GetGroupsByUserEmailAsync(userEmail, searchParameters);
            //assert
            Assert.That(result.TotalCount, Is.EqualTo(groupsCount));
        }
    }
}
