using HelloChatApp.Server.Hubs;
using HelloChatApp.Shared.Abstractions;
using HelloChatApp.Shared.Messages;
using Microsoft.AspNetCore.SignalR;

namespace HelloChatApp.Server.Consumers
{
    public class LastStockPriceConsumer : IConsumer<LastStockPrice>
    {
        private const string StockPriceMessagePattern = "{0} quote is ${1} per share";
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<LastStockPriceConsumer> _logger;        

        public LastStockPriceConsumer(IHubContext<ChatHub> hubContext, ILogger<LastStockPriceConsumer> logger)
        {
            _hubContext=hubContext;
            _logger=logger;
        }

        public Task Consume(LastStockPrice message)
        {
            var stockPriceMessage = string.Format(StockPriceMessagePattern, message.StockCode, message.StockPrice);

            var client = _hubContext.Clients.Client(message.UserHubId);
            if (client == null)
            {
                _logger.LogDebug($"Received message from bot: {stockPriceMessage} - Will not send to {message.UserName}, because couldn't find connection id: {message.UserHubId} session.");
                return Task.CompletedTask;
            }

            _logger.LogDebug($"Received message from bot: {stockPriceMessage} - Sending to {message.UserName}.");
            return client.SendAsync(message.Room, message.UserName, stockPriceMessage);
        }
    }
}
