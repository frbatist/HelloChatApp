using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Models;
using HelloChatApp.Server.Services;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.Server.Services
{
    public class MessageProcessorTest
    {
        private readonly IMessageSenderFactory _messageSenderFactory;
        private readonly IMessageProcessor _messageProcessor;

        public MessageProcessorTest()
        {
            _messageSenderFactory = Substitute.For<IMessageSenderFactory>();
            _messageProcessor = new MessageProcessor(_messageSenderFactory);
        }

        [Fact]
        public async Task ProcessMessage_should_call_sendmessage_with_provided_message_sender()
        {             
            //Arrange
            var room = "A1";            
            var message = "/stock=EET_20";
            var userName = "Connor";
            var userHubId = "1234";            

            var messageDetails = new MessageDetails(room, message, userName, userHubId);
            var messageSender = Substitute.For<IMessageSender>();
            _messageSenderFactory.Get(messageDetails).Returns(messageSender);

            //Act
            await _messageProcessor.ProcessMessage(messageDetails);

            //Assert                        
            await messageSender.Received(1).SendMessage(Arg.Is<MessageDetails>
                (
                    d =>
                    d.Room == room &&
                    d.Message == message &&
                    d.UserName == userName &&
                    d.UserHubId == userHubId
                ));

        }
    }
}
