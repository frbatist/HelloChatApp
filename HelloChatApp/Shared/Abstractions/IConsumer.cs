namespace HelloChatApp.Shared.Abstractions
{
    public interface IConsumer<TMessage>
    {
        Task Consume(TMessage message);
    }
}
