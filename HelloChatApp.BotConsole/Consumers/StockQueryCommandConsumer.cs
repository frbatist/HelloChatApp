using HelloChatApp.BotConsole.Domain.Abstractions;
using HelloChatApp.Shared.Abstractions;
using HelloChatApp.Shared.Messages;

namespace HelloChatApp.BotConsole.Consumers
{
    public class StockQueryCommandConsumer : IConsumer<StockQueryCommand>
    {
        private const string LastStockPriceExchange = "last-stock-price";
        private readonly IStockService _stockService;
        private readonly IPublisher _publisher;

        public StockQueryCommandConsumer(IStockService stockService, IPublisher publisher)
        {
            _stockService = stockService;
            _publisher = publisher;
        }

        public async Task Consume(StockQueryCommand message)
        {
            if (string.IsNullOrEmpty(message.StockCode))
                return;
            var price = await _stockService.GetStockPrice(message.StockCode);
            var lastStockPrice = new LastStockPrice
            {
                UserName = message.UserName,
                UserHubId = message.UserHubId,
                StockCode = message.StockCode,
                StockPrice = price
            };
            _publisher.Publish(LastStockPriceExchange, lastStockPrice);
        }
    }
}
