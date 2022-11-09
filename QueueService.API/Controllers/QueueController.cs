using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QueueService.BL.Dto.Request;
using QueueService.BL.Services;
using QueueService.BL.Dto.Response;

namespace QueueService.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public QueueController(RabbitMQPublisher rabbitMQPublisher)
        {
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpPost("products")]
        public async Task<IActionResult> PublishAsync([FromBody] ProductCreatedMessage productCreatedMessage)
        {
            try
            {
                var response = await _rabbitMQPublisher.PublishMessage(productCreatedMessage);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

    }
}
