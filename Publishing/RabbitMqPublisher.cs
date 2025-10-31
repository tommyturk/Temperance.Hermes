using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Temperance.Hermes.Connection;

namespace Temperance.Hermes.Publishing
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly IRabbitMqConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqPublisher> _logger;

        public RabbitMqPublisher(IRabbitMqConnectionFactory connectionFactory, ILogger<RabbitMqPublisher> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task PublishAsync<T>(string queueName, T message)
        {
            // get connection
            var connection = await _connectionFactory.GetConnection();

            // Create channel
            // channels are NOT thread-safe - so we create one per publish.
            await using var channel = await connection.CreateChannelAsync();

            // declare queue async
            await channel.QueueDeclareAsync(queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var messageBody = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageBody);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(exchange: string.Empty,
                             mandatory: false,
                             routingKey: queueName,
                             basicProperties: properties,
                             body: body);
        }
    }
}
