using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using MediatR;
using Ordering.Core.Repositories;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using EventBusRabbitMQ.Events;
using Ordering.Application.Commands;

namespace Ordering.API.RabbitMQ
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, IMediator mediator, IMapper mapper, IOrderRepository repository)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.BasketCheckoutQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += RecivedEvent;

            channel.BasicConsume(queue: EventBusConstants.BasketCheckoutQueue, autoAck: true, consumer: consumer, noLocal: false, exclusive:false, arguments: null) ;

        }

        private async void RecivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if(e.RoutingKey == EventBusConstants.BasketCheckoutQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                var command = _mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);

                var result = await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }

    }
}
