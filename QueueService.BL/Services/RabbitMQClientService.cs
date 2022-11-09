using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.BL.Services
{
    public class RabbitMQClientService : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMQClientService> _logger;

        public static string ExchangeName = "exchange";
        public static string QueueName = "product.new";
        public static string RoutingName = "route.product";

        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if(_channel is { IsOpen:true})
            {
                return _channel;
            }

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: ExchangeName,
                                     type: "direct",
                                     durable: true,
                                     autoDelete: false);

            _channel.QueueDeclare(queue: QueueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false);

            _channel.QueueBind(exchange: ExchangeName,
                               queue: QueueName,
                               routingKey: RoutingName);

            _logger.LogInformation("Successfully connected to the RabbitMQ");

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("Disconnected from RabbitMQ");
        }
    }
}
