using HelloChatApp.BotConsole.Domain.Abstractions;
using Microsoft.Extensions.Configuration;

namespace HelloChatApp.BotConsole.Infra.Services
{
    public class StockRepository : IStockRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private const string StockServiceUrlKey = "StockServiceUrl";

        public StockRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public Task<string> GetStock(string stockCode)
        {
            var url = _configuration.GetValue<string>(StockServiceUrlKey);
            if (url == null)
                return Task.FromResult(string.Empty);

            return _httpClient.GetStringAsync(url);
        }
    }
}
