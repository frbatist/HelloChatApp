namespace HelloChatApp.BotConsole.Abstractions
{
    internal interface IPublisher
    {
        void Publish<T>(string exchangeName, T message);
    }
}
