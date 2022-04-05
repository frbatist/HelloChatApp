using FluentAssertions;
using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Models;
using HelloChatApp.Server.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.Server.Services
{
    public class MessageSenderFactoryTest
    {
        private readonly ICommandMessageSender _commandMessageSender;
        private readonly IChatMessageSender _chatMessageSender;
        private readonly IMessageSenderFactory _messageSenderFactory;

        public MessageSenderFactoryTest()
        {
            _commandMessageSender = Substitute.For<ICommandMessageSender>();
            _chatMessageSender = Substitute.For<IChatMessageSender>();
            _messageSenderFactory = new MessageSenderFactory(_commandMessageSender, _chatMessageSender);
        }

        [Fact]
        public void Get_shoul_return_ICommandMessageSender_as_message_is_a_command()
        {
            //Arrange
            var room = "A1";
            var message = "/stock=EET_20";
            var userName = "Connor";
            var userHubId = "1234";
            var messageDetails = new MessageDetails(room, message, userName, userHubId);

            //Act
            var instance = _messageSenderFactory.Get(messageDetails);

            //Assert
            instance.Should().Be(_commandMessageSender);
        }

        [Fact]
        public void Get_Get_shoul_return_IChatMessageSender_as_message_is_not_a_command()
        {
            //Arrange
            var room = "A1";
            var message = "hello my friend, stay a while and listen";
            var userName = "Connor";
            var userHubId = "1234";
            var messageDetails = new MessageDetails(room, message, userName, userHubId);

            //Act
            var instance = _messageSenderFactory.Get(messageDetails);

            //Assert
            instance.Should().Be(_chatMessageSender);
        }
    }
}
