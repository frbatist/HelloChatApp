using RabbitMQ.Client;

namespace HelloChatApp.BotConsole.Infra.RabbitMq
{
    public interface IRabbitMqConnection
    {
        IConnection GetConnection();
    }
}
