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
            //Act
            var messages = await _messageRepository.GetAllByGroupIdAsync(1);
            var message2 = await _messageRepository.GetAllAsync();
            var ms2 = message2.Count();
            //Assert
            Assert.That(messages.Count(), Is.EqualTo(3));
        }
    }
}
