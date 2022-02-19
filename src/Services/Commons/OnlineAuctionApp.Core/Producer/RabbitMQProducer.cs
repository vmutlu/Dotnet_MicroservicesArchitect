using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlineAuctionApp.Core.Abstract;
using OnlineAuctionApp.Core.Event.Abstract;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
namespace OnlineAuctionApp.Core.Producer
{
    public class RabbitMQProducer
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private readonly ILogger<RabbitMQProducer> _logger;
        private readonly int _retryCount;

        public RabbitMQProducer(IRabbitMQConnection rabbitMQConnection, ILogger<RabbitMQProducer> logger, int retryCount = 5)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _logger = logger;
            _retryCount = retryCount;
        }

        public void Publish(string queueName, IEvent @event)
        {
            if (!_rabbitMQConnection.IsConnected) _rabbitMQConnection.TryConnect();

            var policy = RetryPolicy.Handle<SocketException>().Or<BrokerUnreachableException>().WaitAndRetry(_retryCount, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)), (ex, time) =>
            {
                _logger.LogWarning(ex, "RabbitMQ could not connect after {TimeOut}s ({ExxceptionMessage})", $"{ time.TotalSeconds:n1}", ex.Message);
            });

            using var channel = _rabbitMQConnection.CreateModel();
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                    );
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Debug.WriteLine("Sent RabbitMQ");
                };
            });
        }
    }
}
