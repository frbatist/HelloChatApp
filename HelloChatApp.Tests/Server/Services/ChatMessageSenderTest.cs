using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Hubs;
using HelloChatApp.Server.Models;
using HelloChatApp.Server.Services;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.Server.Services
{
    public class ChatMessageSenderTest
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IClientProxy _clientProxy;
        private readonly IHubClients _hubClients;
        private readonly IMessageSender _messageSender;

        public ChatMessageSenderTest()
        {
            _hubContext = Substitute.For<IHubContext<ChatHub>>();
            _clientProxy = Substitute.For<IClientProxy>();
            _hubClients = Substitute.For<IHubClients>();
            _messageSender = new ChatMessageSender(_hubContext);
        }

        [Fact]
        public async Task SendMessage_shoul_send_message_to_all_users_on_the_room()
        {
            //Arrange
            var room = "A1";
            var message = "how nice!";
            var userName = "Connor";
            var userHubId = "1234";

            _hubClients.All.Returns(_clientProxy);
            _hubContext.Clients.Returns(_hubClients);

            var messageDetails = new MessageDetails(room, message, userName, userHubId);

            //Act
            await _messageSender.SendMessage(messageDetails);

            //Assert
            await _clientProxy.Received(1).SendCoreAsync(room, Arg.Is<object[]>
                (
                    d => AssertSendMessage(d, userName, message)
                ),
                Arg.Any<CancellationToken>()
            );            
        }

        private static bool AssertSendMessage(object[] parameters, string userName, string message)
        {
            return parameters[0].ToString() == userName && parameters[1].ToString() == message;
        }
    }
}
