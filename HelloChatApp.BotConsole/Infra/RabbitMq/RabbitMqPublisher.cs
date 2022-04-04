using HelloChatApp.BotConsole.Abstractions;
using RabbitMQ.Client;
using System.Text.Json;

namespace HelloChatApp.BotConsole.Infra.RabbitMq
{
    public class RabbitMqPublisher : IPublisher
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;

        public RabbitMqPublisher(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void Publish<T>(string exchangeName, T message)
        {
            var conn = _rabbitMqConnection.GetConnection();
            var channel = conn.CreateModel();
            var body = JsonSerializer.SerializeToUtf8Bytes(message);
            channel.BasicPublish(exchange: "logs",
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);            
        }
    }
}
