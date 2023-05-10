using Chat.DAL.Data;
using Chat.DAL.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Tests.Repository.Tests
{
    [TestFixture]
    public class MessageRepositoryTests
    {
        MessageRepository _messageRepository;

        [SetUp]
        public void Setup()
        {
            var context = new ChatDbContext(TestHelper.GetUnitTestDbOptions());
            _messageRepository = new(context);
        }

        [Test]
        public async Task GetAllMessagesByGroupId_ReturnsMessages()
        {
            //Arrange
            int groupId = 1;
            //Act
            var messages = await _messageRepository.GetAllByGroupIdAsync(groupId);
            var expected = (await _messageRepository.GetAllAsync()).Where(m => m.GroupId== groupId).ToList();
            //Assert
            Assert.That(messages.Count, Is.EqualTo(expected.Count));
        }
    }
}
