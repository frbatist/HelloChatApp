using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace HelloChatApp.Shared.Infra.RabbitMq
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConnection _connection;
        private const string RabbitMqConnectionStringKey = "RabbitMqConnection";

        public RabbitMqConnection(IConfiguration configuration)
        {
            var rabbitConnectionString = configuration.GetValue<string>(RabbitMqConnectionStringKey);

            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new UriBuilder(rabbitConnectionString).Uri,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return _connection;
        }
    }
}
