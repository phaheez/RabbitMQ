using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using RabbitMQSample.Models;
using RabbitMQSample.Services;

namespace RabbitMQSample.Controllers
{
    [Route("api")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IBus _busControl;

        public TestController(IBus busControl)
        {
            _busControl = busControl;
        }

        [HttpPost("message/send")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            try
            {
                message.Id = Guid.NewGuid().ToString();

                await _busControl.SendAsync("demo-queue", message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Ok("Message sent to RabbitMQ");
        }
    }
}
