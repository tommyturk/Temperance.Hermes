using RabbitMQ.Client;

namespace Temperance.Hermes.Connection
{
    public interface IRabbitMqConnectionFactory : IAsyncDisposable
    {
        Task<IConnection> GetConnection();
    }
}
