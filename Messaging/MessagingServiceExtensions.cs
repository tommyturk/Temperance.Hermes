using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Temperance.Hermes.Configuration;
using Temperance.Hermes.Connection;
using Temperance.Hermes.Publishing;

namespace Temperance.Hermes.Messaging
{
    public static class MessagingServiceExtensions
    {
        public static IServiceCollection AddTemperanceMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
            services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
            return services;
        }
    }
}
