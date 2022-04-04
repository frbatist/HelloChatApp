using HelloChatApp.BotConsole.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HelloChatApp.BotConsole
{
    public static class RabbitMqMessageSubscriber
    {
        private const string RabbitMqConnectionStringKey = "RabbitMqConnection";

        public static void Subscribe<TMessage>(this IServiceProvider serviceProvider, string exchange, string queue)
        {
            IConfiguration config = serviceProvider.GetRequiredService<IConfiguration>();
            var rabbitConnectionString = config.GetValue<string>(RabbitMqConnectionStringKey);

            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new UriBuilder(rabbitConnectionString).Uri,
                DispatchConsumersAsync = true
            };

            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.ExchangeDeclare(exchange, ExchangeType.Fanout, true, false);
            channel.QueueDeclare(queue, true, false, false, null);
            channel.QueueBind(queue, exchange, "#", null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (ch, ea) =>
            {                
                var message = JsonSerializer.Deserialize<TMessage>(ea.Body.ToArray());
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var consumer = serviceScope.ServiceProvider.GetService<IConsumer<TMessage>>();
                    await consumer.Consume(message);
                }
                channel.BasicAck(ea.DeliveryTag, false);
                await Task.Yield();

            };
            channel.BasicConsume(queue, false, consumer);
            Thread.Sleep(8000);
        }
    }
}
