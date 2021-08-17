using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQSample.Services
{
    public interface IBus
    {
        Task SendAsync<T>(string queue, T message);
    }
}
