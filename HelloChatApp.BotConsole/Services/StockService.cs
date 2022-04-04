using HelloChatApp.BotConsole.Domain.Abstractions;
using System.Globalization;

namespace HelloChatApp.BotConsole.Services
{
    public class StockService : IStockService
    {
        private const int CsvStockPriceIndex = 6;
        private const string CsvLineSeparator = "\r\n";
        private const string CsvColumnSeparator = ",";
        private const string CsvCulture = "en-us";        
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {            
            _stockRepository = stockRepository;
        }

        public async Task<double> GetStockPrice(string stockCode)
        {
            var stockCsv = await _stockRepository.GetStock(stockCode);
            if (string.IsNullOrEmpty(stockCsv))
            { 
                return 0;
            }
            var lines = stockCsv.Split(CsvLineSeparator);
            var lastLine = lines[lines.Length - 1];
            var columns = lastLine.Split(CsvColumnSeparator);

            if (columns.Length < CsvStockPriceIndex)
                return 0;
            var column = columns[CsvStockPriceIndex];

            if (!double.TryParse(column, out _))
                return 0;
            
            return double.Parse(columns[CsvStockPriceIndex], new CultureInfo(CsvCulture));
        }
    }
}
