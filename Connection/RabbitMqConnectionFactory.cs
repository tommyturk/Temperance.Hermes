using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Temperance.Hermes.Configuration;

namespace Temperance.Hermes.Connection
{
    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        private readonly ILogger<RabbitMqConnectionFactory> _logger;
        private readonly IConnectionFactory _factory;
        private readonly Lazy<Task<IConnection>> _lazyConnection;
        public RabbitMqConnectionFactory(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqConnectionFactory> logger)
        {
            _logger = logger;
            var config = settings.Value;

            var factory = new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password,
            };

            _factory = factory;
            _lazyConnection = new Lazy<Task<IConnection>>(() => CreateConnectionInternalAsync());
        }
       
        private async Task<IConnection> CreateConnectionInternalAsync()
        {
            var connection = await _factory.CreateConnectionAsync();
            return connection;
        }

        public Task<IConnection> GetConnection()
        {
            return _lazyConnection.Value;
        }

        public async ValueTask DisposeAsync()
        {
            if (_lazyConnection.IsValueCreated)
            {
                var connection = await _lazyConnection.Value;
                connection?.CloseAsync();
                connection?.DisposeAsync();
            }
        }
    }
}
