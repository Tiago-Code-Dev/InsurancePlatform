using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using IRabbitConnection = RabbitMQ.Client.IConnection;
using IRabbitModel = RabbitMQ.Client.IModel;

namespace Shared.CrossCutting.Messaging.RabbitMq;

public class RabbitMqEventBus : IEventBus, IDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly IRabbitConnection _connection;
    private readonly IRabbitModel _channel;

    public RabbitMqEventBus(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;

        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: _options.Exchange,
            type: ExchangeType.Direct,
            durable: _options.Durable,
            autoDelete: false
        );
    }

    public Task PublishAsync<T>(T @event, string queueName) where T : class
    {
        _channel.QueueDeclare(
            queue: queueName,
            durable: _options.Durable,
            exclusive: false,
            autoDelete: false
        );
        _channel.QueueBind(queue: queueName, exchange: _options.Exchange, routingKey: queueName);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        var props = _channel.CreateBasicProperties();
        props.DeliveryMode = 2;

        _channel.BasicPublish(
            exchange: _options.Exchange,
            routingKey: queueName,
            basicProperties: props,
            body: body
        );

        return Task.CompletedTask;
    }

    public Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class
    {
        _channel.QueueDeclare(queue: queueName, durable: _options.Durable, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue: queueName, exchange: _options.Exchange, routingKey: queueName);
        _channel.BasicQos(0, _options.PrefetchCount, false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var message = JsonSerializer.Deserialize<T>(json);

            if (message is not null)
                await handler(message);

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
