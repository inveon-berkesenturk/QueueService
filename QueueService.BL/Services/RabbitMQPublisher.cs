using QueueService.BL.Dto.Request;
using QueueService.BL.Dto.Response;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QueueService.BL.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public async Task<RabbitMQPublishQueueMessageResponse> PublishMessage(ProductCreatedMessage productCreatedMessage)
        {
            var channel = _rabbitMQClientService.Connect();

            var bodyString = JsonSerializer.Serialize(productCreatedMessage);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();

            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName,
                                 routingKey: RabbitMQClientService.RoutingName,
                                 basicProperties: properties,
                                 body: bodyByte);
            
            return new RabbitMQPublishQueueMessageResponse
            {
                Success = true,
                Message = "Message has been queued"
            };

        }
    }
}
