using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MotorcycleRentalSystem.Infrastructure.Messaging
{
    public class RabbitMqClient<T> : IRabbitMqClient<T>, IDisposable
    {
        private readonly IRabbitMqSettings _settings;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMqClient(IRabbitMqSettings settings)
        {
            _settings = settings;

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_settings.ConnectionString)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_settings.ExchangeName, ExchangeType.Direct);

            // Define o nome da fila explicitamente
            _queueName = _settings.QueueName; // Exemplo: "MotorcycleRegisteredQueue"
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);

            // Vincula a fila ao exchange com a chave de roteamento padrão
            _channel.QueueBind(queue: _queueName, exchange: _settings.ExchangeName, routingKey: _settings.RoutingKey);
        }

        public async Task PublishAsync(T message, string routingKey)
        {
            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            _channel.BasicPublish(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body
            );

            await Task.CompletedTask;
        }

        public void Subscribe(Func<T, Task> onMessageReceived)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));
                if (message != null)
                {
                    await onMessageReceived(message);
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
