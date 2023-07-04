using Chat.BLL.DTO;
using Chat.BLL.Services.Implementation;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.DAL.Repositories.Interfaces;
using Chat.DAL.UoW.Interface;
using Moq;

namespace Chat.Tests.Services.Tests
{
    [TestFixture]
    public class MessageServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IMessageService _messageService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _messageService = new MessageService(_mockUnitOfWork.Object, TestHelper.CreateMapperProfile());
        }

        [Test]
        public async Task AddNewMessageAsync_AddsNewMessage()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IMessageRepository>().AddAsync(It.IsAny<Message>()));
            var newMessage = new MessageDTO() {Id = 1};
            //act
            await _messageService.AddNewMessageAsync(newMessage);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IMessageRepository>()
                .AddAsync(It.IsAny<Message>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllMessagesByGroupIdasync_ReturnsMessages()
        {
            //arrange
            _mockUnitOfWork.Setup(x => x.GetRepository<IMessageRepository>().GetAllByGroupIdAsync(It.IsAny<int>()));
            int groupId = 1;
            //act
            await _messageService.GetAllMessagesByGroupIdasync(groupId);
            //assert
            _mockUnitOfWork.Verify(x => x.GetRepository<IMessageRepository>()
                .GetAllByGroupIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
