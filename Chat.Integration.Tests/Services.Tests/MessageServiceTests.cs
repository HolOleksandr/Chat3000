using Chat.BLL.DTO;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Integration.Tests.Services.Tests
{
    [TestFixture]
    public class MessageServiceTests
    {
        private IMessageService _messageService;

        [SetUp]
        public void Setup()
        {
            var helper = new TestHelpers();
            _messageService = helper.GetMessageService();
            TestHelpers.ResetMessageTable();
        }

        [TestCase(1, 3)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(100500, 0)]
        public async Task GetAllMessagesByGroupIdasync_ReturnsMessages(int groupId, int expectedCount)
        {
            //arrange
            //act
            var messages = await _messageService.GetAllMessagesByGroupIdasync(groupId);
            //assert
            Assert.That(messages.ToList(), Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public async Task AddNewMessageAsync_AddsNewMessages()
        {
            //arrange
            var newMessage = new MessageDTO() { GroupId = 1, SenderId = "57f9b20f-3d14-4f48-be5f-90084218b437", Text = "Hello" };
            //act
            var messagesCount = (await _messageService.GetAllMessagesByGroupIdasync(newMessage.GroupId)).ToList().Count;
            await _messageService.AddNewMessageAsync(newMessage);
            var allMessages = (await _messageService.GetAllMessagesByGroupIdasync(newMessage.GroupId)).ToList();
            var newAddedMessage = allMessages.OrderByDescending(x => x.SendDate).First();
            //assert
            Assert.Multiple(() =>
            {
                Assert.That(allMessages, Has.Count.EqualTo(messagesCount + 1));
                Assert.That(newAddedMessage.SenderId, Is.EqualTo(newMessage.SenderId));
                Assert.That(newAddedMessage.Text, Is.EqualTo(newMessage.Text));
            });
        }
    }
}
