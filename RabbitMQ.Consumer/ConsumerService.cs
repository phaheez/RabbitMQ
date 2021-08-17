using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Consumer
{
    public class ConsumerService : BackgroundService
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ApplicationSettings _appSettings;

        public ConsumerService(IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(ApplicationSettings));
            _factory = new ConnectionFactory
            {
                HostName = _appSettings.RabbitMQ.QueueConnection,
                DispatchConsumersAsync = true
            };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            // read the message bytes
            var body = e.Body.ToArray();

            // convert back to the original string
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);

            _channel.BasicAck(e.DeliveryTag, false);

            await Task.Yield();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueName = _appSettings.RabbitMQ.QueueName;

            _channel.QueueDeclare(queueName, true, false, false, null);

            // get total number of message in queue
            var queuePassive = _channel.QueueDeclarePassive(queueName);
            var messageCount = Convert.ToInt16(queuePassive.MessageCount);
            var consumerCount = Convert.ToInt16(queuePassive.ConsumerCount);
            Console.WriteLine($"This channels has {messageCount} messages and {consumerCount} consumer(s) on the queue");

            // create a consumer that listens on the channel (queue)
            var consumer = new AsyncEventingBasicConsumer(_channel);

            // handle the Received event on the consumer
            // this is triggered whenever a new message
            // is added to the queue by the producer
            consumer.Received += Consumer_Received;

            _channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Consumer started");

            //await Task.Yield();
            await Task.CompletedTask;
        }
        
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _channel.Dispose();
            _connection.Dispose();

            await Task.CompletedTask;
        }
    }
}
