namespace HelloChatApp.BotConsole.Abstractions
{
    public interface IStockRepository
    {
        Task<string> GetStock(string stockCode);
    }
}
