namespace HelloChatApp.BotConsole.Abstractions
{
    public interface IPublisher
    {
        void Publish<T>(string exchangeName, T message);
    }
}
