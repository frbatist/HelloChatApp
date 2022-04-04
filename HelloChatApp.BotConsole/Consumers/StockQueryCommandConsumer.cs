using HelloChatApp.BotConsole.Abstractions;
using HelloChatApp.BotConsole.Messages;

namespace HelloChatApp.BotConsole.Consumers
{
    public class StockQueryCommandConsumer : IConsumer<StockQueryCommand>
    {
        public Task Consume(StockQueryCommand message)
        {
            Console.WriteLine($"Consumed command: {message.StockId}");
            return Task.CompletedTask;
        }
    }
}
