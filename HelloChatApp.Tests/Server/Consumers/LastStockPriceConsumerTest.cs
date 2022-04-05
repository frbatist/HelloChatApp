using HelloChatApp.Server.Consumers;
using HelloChatApp.Server.Hubs;
using HelloChatApp.Shared.Messages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.Server.Consumers
{
    public class LastStockPriceConsumerTest
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IClientProxy _clientProxy;
        private readonly IHubClients _hubClients;
        private readonly ILogger<LastStockPriceConsumer> _logger;
        private readonly LastStockPriceConsumer _lastStockPriceConsumer;

        public LastStockPriceConsumerTest()
        {
            _hubContext = Substitute.For<IHubContext<ChatHub>>();
            _clientProxy = Substitute.For<IClientProxy>();
            _hubClients = Substitute.For<IHubClients>();
            _logger = Substitute.For<ILogger<LastStockPriceConsumer>>();
            _lastStockPriceConsumer = new LastStockPriceConsumer(_hubContext, _logger);
        }

        [Fact]
        public async Task Consume_should_send_message_with_stock_values_for_user()
        {
            //Arrange
            var room = "houseofmotherjoana";
            var username = "Fagner";
            var userHubId = "09876";
            var stockCode = "AAPL.US";
            var stockPrice = 120;
            var message = $"{stockCode} quote is ${stockPrice} per share";

            var lastStockPrice = new LastStockPrice
            {
                Room = room,
                UserName = username,
                UserHubId = userHubId,
                StockCode = stockCode,
                StockPrice = stockPrice                                
            };

            _hubClients.Client(userHubId).Returns(_clientProxy);
            _hubContext.Clients.Returns(_hubClients);

            //Act
            await _lastStockPriceConsumer.Consume(lastStockPrice);

            //Assert
            await _clientProxy.Received(1).SendCoreAsync(room, Arg.Is<object[]>
                (
                    d => AssertSendMessage(d, username, message)
                ),
                Arg.Any<CancellationToken>()
            );
            _logger.Received(1).LogDebug($"Received message from bot: {message} - Sending to {username}.");
            
        }

        [Fact]
        public async Task Consume_should_ignore_message_if_the_requesting_user_is_not_available_anymore()
        {
            //Arrange
            var room = "houseofmotherjoana";
            var username = "Fagner";
            var userHubId = "09876";
            var stockCode = "AAPL.US";
            var stockPrice = 120;
            var message = $"{stockCode} quote is ${stockPrice} per share";

            var lastStockPrice = new LastStockPrice
            {
                Room = room,
                UserName = username,
                UserHubId = userHubId,
                StockCode = stockCode,
                StockPrice = stockPrice
            };
            _hubClients.Client(userHubId).Returns(default(IClientProxy));
            _hubContext.Clients.Returns(_hubClients);

            //Act
            await _lastStockPriceConsumer.Consume(lastStockPrice);

            //Assert            
            _logger.Received(1).LogDebug($"Received message from bot: {message} - Will not send to {username}, because couldn't find connection id: {userHubId} session.");

        }

        private static bool AssertSendMessage(object[] parameters, string userName, string message)
        {
            return parameters[0].ToString() == userName && parameters[1].ToString() == message;
        }
    }
}
