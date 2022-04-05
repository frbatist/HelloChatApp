using HelloChatApp.Server.Domain.Abstractions;
using HelloChatApp.Server.Hubs;
using HelloChatApp.Server.Models;
using HelloChatApp.Server.Services;
using HelloChatApp.Shared.Abstractions;
using HelloChatApp.Shared.Messages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.Server.Services
{
    public class MessageProcessorTest
    {
        private const string StockQueryCommandExchange = "stock-query-command";
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IClientProxy _clientProxy;
        private readonly IHubClients _hubClients;
        private readonly ILogger<MessageProcessor> _logger;
        private readonly IPublisher _publisher;
        private readonly IMessageProcessor _messageProcessor;

        public MessageProcessorTest()
        {
            _hubContext = Substitute.For<IHubContext<ChatHub>>();
            _clientProxy = Substitute.For<IClientProxy>();
            _hubClients = Substitute.For<IHubClients>();            
            _logger = Substitute.For<ILogger<MessageProcessor>>();
            _publisher = Substitute.For<IPublisher>();
            _messageProcessor = new MessageProcessor(_hubContext, _logger, _publisher);
        }

        [Fact]
        public async Task ProcessMessage_should_send_message_to_all_socket_clients_when_its_not_a_command()
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
            await _messageProcessor.ProcessMessage(messageDetails);

            //Assert
            await _clientProxy.Received(1).SendCoreAsync(room, Arg.Is<object[]>
                (
                    d => AssertSendMessage(d, userName, message)
                ),
                Arg.Any<CancellationToken>()
            );
            _logger.Received(1).LogDebug("Message is not a command!");
            _publisher.Received(0).Publish(Arg.Any<string>(), Arg.Any<StockQueryCommand>());
        }

        [Fact]
        public async Task ProcessMessage_should_send_message_to_broker_when_its_a_command()
        {
            //Arrange
            var room = "A1";
            string stockCode = "EET_20";
            var message = "/stock=EET_20";
            var userName = "Connor";
            var userHubId = "1234";

            _hubClients.All.Returns(_clientProxy);
            _hubContext.Clients.Returns(_hubClients);

            var messageDetails = new MessageDetails(room, message, userName, userHubId);

            //Act
            await _messageProcessor.ProcessMessage(messageDetails);

            //Assert
            await _clientProxy.Received(0).SendCoreAsync(room, Arg.Any<object[]>(), Arg.Any<CancellationToken>());
            _logger.Received(1).LogDebug("Message is a command - stock!");
            _publisher.Received(1).Publish(StockQueryCommandExchange, Arg.Is<StockQueryCommand>
                (
                    d =>
                    d.Room == room &&
                    d.UserName == userName &&
                    d.UserHubId == userHubId &&
                    d.StockCode == stockCode
                ));
        }

        [Fact]
        public async Task ProcessMessage_should_send_error_message_to_user_when_the_provided_command_is_not_valid()
        {
            //Arrange
            var room = "A1";
            string stockCode = "EET_20";
            var message = "/car=EET_20";
            var userName = "Connor";
            var userHubId = "1234";

            _hubClients.Client(userHubId).Returns(_clientProxy);
            _hubContext.Clients.Returns(_hubClients);

            var messageDetails = new MessageDetails(room, message, userName, userHubId);

            //Act
            await _messageProcessor.ProcessMessage(messageDetails);

            //Assert
            await _clientProxy.Received(1).SendCoreAsync(room, Arg.Is<object[]>
                (
                    d => AssertSendMessage(d, userName, "Message is not a valid command - car!")
                ),
                Arg.Any<CancellationToken>()
            );
            _logger.Received(1).LogDebug("Message is not a valid command - car!");
            _publisher.Received(0).Publish(Arg.Any<string>(), Arg.Any<StockQueryCommand>());
        }

        [Theory]
        [InlineData("/stock=")]
        [InlineData("/stock")]
        public async Task ProcessMessage_should_send_error_message_to_user_when_the_provided_command_value_is_null_or_empty(string message)
        {
            //Arrange
            var room = "A1";            
            var userName = "Connor";
            var userHubId = "1234";

            _hubClients.Client(userHubId).Returns(_clientProxy);
            _hubContext.Clients.Returns(_hubClients);

            var messageDetails = new MessageDetails(room, message, userName, userHubId);

            //Act
            await _messageProcessor.ProcessMessage(messageDetails);

            //Assert
            _logger.Received(1).LogDebug("Message is a command - stock!");
            await _clientProxy.Received(1).SendCoreAsync(room, Arg.Is<object[]>
                (
                    d => AssertSendMessage(d, userName, "Command invalid: stock. The value must be informed!")
                ),
                Arg.Any<CancellationToken>()
            );
            _logger.Received(1).LogDebug("Command invalid: stock. The value must be informed!");
            _publisher.Received(0).Publish(Arg.Any<string>(), Arg.Any<StockQueryCommand>());
        }

        private static bool AssertSendMessage(object[] parameters, string userName, string message)
        {
            return parameters[0].ToString() == userName && parameters[1].ToString() == message;
        }
    }
}
