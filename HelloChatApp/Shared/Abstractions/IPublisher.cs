namespace HelloChatApp.Shared.Abstractions
{
    public interface IPublisher
    {
        void Publish<T>(string exchangeName, T message);
    }
}
