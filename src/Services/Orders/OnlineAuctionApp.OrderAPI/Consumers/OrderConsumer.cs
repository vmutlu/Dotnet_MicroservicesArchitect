using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using OnlineAuctionApp.Application.Commands.OrderCreate;
using OnlineAuctionApp.Core.Abstract;
using OnlineAuctionApp.Core.Common;
using OnlineAuctionApp.Core.Event.Concrete;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace OnlineAuctionApp.OrderAPI.Consumers
{
    public class OrderConsumer
    {
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderConsumer(IRabbitMQConnection rabbitMQConnection, IMediator mediator, IMapper mapper)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _mediator = mediator;
            _mapper = mapper;
        }

        public void Consume()
        {
            if (!_rabbitMQConnection.IsConnected)
                _rabbitMQConnection.TryConnect();

            var channel = _rabbitMQConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.ORDER_CREATE, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceiveEvent;
            channel.BasicConsume(queue: EventBusConstants.ORDER_CREATE, autoAck: true, consumer: consumer);
        }

        //Gelen mesajı işler
        private async void ReceiveEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<OrderCreatedEvent>(message);

            if (e.RoutingKey == EventBusConstants.ORDER_CREATE)
            {
                var command = _mapper.Map<OrderCreateCommand>(@event);

                command.CreatedDate = DateTime.UtcNow;
                command.TotalPrice = @event.Quantity * @event.Price;
                command.UnitPrice = @event.Price;

                await _mediator.Send(command).ConfigureAwait(false);
            }
        }

        public void Disconnected() => _rabbitMQConnection.Dispose();
    }
}
