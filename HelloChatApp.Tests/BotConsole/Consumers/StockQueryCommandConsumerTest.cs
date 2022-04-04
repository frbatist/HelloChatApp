using HelloChatApp.BotConsole.Consumers;
using HelloChatApp.BotConsole.Domain.Abstractions;
using HelloChatApp.Shared.Abstractions;
using HelloChatApp.Shared.Messages;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.BotConsole.Consumers
{
    public class StockQueryCommandConsumerTest
    {
        private const string LastStockPriceExchange = "last-stock-price";
        private readonly IStockService _stockService;
        private readonly IPublisher _publisher;
        private readonly StockQueryCommandConsumer _stockQueryCommandConsumer;

        public StockQueryCommandConsumerTest()
        {
            _stockService = Substitute.For<IStockService>();
            _publisher = Substitute.For<IPublisher>();
            _stockQueryCommandConsumer = new StockQueryCommandConsumer(_stockService, _publisher);
        }

        [Fact]
        public async Task Consume_should_get_stock_price_and_send_ti_back_to_chat_app()
        {
            //Arrange            
            var username = "Fagner";
            var userHubId = "09876";
            var stockCode = "AAPL.US";
            var stockPrice = 120;

            var command = new StockQueryCommand
            {
                UserName = username,
                StockCode = stockCode,
                UserHubId = userHubId
            };

            _stockService.GetStockPrice(stockCode).Returns(stockPrice);

            //Act
            await _stockQueryCommandConsumer.Consume(command);

            //Assert
            _publisher.Received(1).Publish(LastStockPriceExchange, Arg.Is<LastStockPrice>
            (
                d => d.UserName == username &&
                d.StockCode == stockCode &&
                d.StockPrice == stockPrice &&
                d.UserHubId == userHubId
            ));            
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Consume_should_not_search_and_send_price_to_broker_if_stock_code_is_null_or_empty(string stockCode)
        {
            //Arrange            
            var username = "Fagner";
            var userHubId = "09876";            
            
            var command = new StockQueryCommand
            {
                UserName = username,
                StockCode = stockCode,
                UserHubId = userHubId
            };

            //Act
            await _stockQueryCommandConsumer.Consume(command);

            //Assert
            _publisher.Received(0).Publish(LastStockPriceExchange, Arg.Any<LastStockPrice>());            
        }
    }
}