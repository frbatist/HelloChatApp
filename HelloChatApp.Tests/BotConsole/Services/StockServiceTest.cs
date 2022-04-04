using FluentAssertions;
using HelloChatApp.BotConsole.Domain.Abstractions;
using HelloChatApp.BotConsole.Services;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.BotConsole.Services
{
    public class StockServiceTest
    {        
        private readonly IStockRepository _stockRepository;
        private readonly IStockService _stockService;

        public StockServiceTest()
        {            
            _stockRepository = Substitute.For<IStockRepository>();
            _stockService = new StockService(_stockRepository);
        }

        [Fact]
        public async Task GetStockPrice_should_return_the_price_of_the_provided_stock() 
        {
            //Arrange
            var stockCsv = "Symbol,Date,Time,Open,High,Low,Close,Volume"
                + Environment.NewLine
                + "AAPL.US,2022-04-04,17:40:18,174.57,178.21,174.44,177.28,27663547";
            var stockCode = "AAPL.US";
            var stockPrice = 177.28;
            _stockRepository.GetStock(stockCode).Returns(stockCsv);

            //Act
            var price = await _stockService.GetStockPrice(stockCode);

            //Arrange
            price.Should().Be(stockPrice);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Symbol,Date,Time,Open,High,Low,Close,Volume")]
        [InlineData("Symbol,Date,Time,Open,High,Low,Close,Volume\r\n12,13,14")]
        public async Task GetStockPrice_should_return_zero_if_the_api_returns_a_invalid_csv(string stockCsv)
        {
            //Arrange      
            var stockCode = "AAPL.US";
            var stockPrice = 0;
            _stockRepository.GetStock(stockCode).Returns(stockCsv);

            //Act
            var price = await _stockService.GetStockPrice(stockCode);

            //Arrange
            price.Should().Be(stockPrice);
        }
    }
}
