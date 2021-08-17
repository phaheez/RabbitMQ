using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer
{
    public class ApplicationSettings
    {
        public RabbitMQ RabbitMQ { get; set; }
    }

    public class RabbitMQ
    {
        public string QueueConnection { get; set; }
        public string QueueName { get; set; }
    }
}
