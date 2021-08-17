using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQSample
{
    public class ApplicationSettings
    {
        public RabbitMQ RabbitMQ { get; set; }
    }

    public class RabbitMQ
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
