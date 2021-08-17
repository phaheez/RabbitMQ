using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQSample.Services
{
    public class RabbitBus: IBus
    {
        private readonly IModel _channel;
        internal RabbitBus(IModel channel)
        {
            _channel = channel;
        }

        public async Task SendAsync<T>(string queue, T message)
        {
            await Task.Run(() =>
            {
                _channel.QueueDeclare(queue, true, false, false, null);
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = false;
                var output = JsonConvert.SerializeObject(message);
                _channel.BasicPublish(string.Empty, queue, null,
                    Encoding.UTF8.GetBytes(output));
            });
        }
    }
}
