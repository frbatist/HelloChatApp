using HelloChatApp.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace HelloChatApp.Shared.Infra.RabbitMq
{
    public static class RabbitMqMessageSubscriber
    {
        public static void Subscribe<TMessage>(this IServiceProvider serviceProvider, string exchange, string queue)
        {
            var rabbitMqConnection = serviceProvider.GetRequiredService<IRabbitMqConnection>();
            IConnection conn = rabbitMqConnection.GetConnection();
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
        }
    }
}
