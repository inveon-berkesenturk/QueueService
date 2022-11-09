using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueService.BL.Dto.Response
{
    public class RabbitMQPublishQueueMessageResponse : BaseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
