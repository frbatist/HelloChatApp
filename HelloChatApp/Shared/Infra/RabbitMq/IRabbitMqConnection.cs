using RabbitMQ.Client;

namespace HelloChatApp.Shared.Infra.RabbitMq
{
    public interface IRabbitMqConnection
    {
        IConnection GetConnection();
    }
}
