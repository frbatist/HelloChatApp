namespace HelloChatApp.BotConsole.Abstractions
{
    public interface IConsumer<TMessage>
    {
        Task Consume(TMessage message);
    }
}
