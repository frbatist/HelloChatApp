namespace HelloChatApp.BotConsole.Domain.Abstractions
{
    public interface IStockRepository
    {
        Task<string> GetStock(string stockCode);
    }
}
