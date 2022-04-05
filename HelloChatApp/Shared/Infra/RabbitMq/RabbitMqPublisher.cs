using HelloChatApp.Shared.Abstractions;
using RabbitMQ.Client;
using System.Text.Json;

namespace HelloChatApp.Shared.Infra.RabbitMq
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
            channel.BasicPublish(exchange: exchangeName,
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
