namespace ContractingService.Infrastructure.Messaging;

using ContractingService.Application.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.CrossCutting.Messaging.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqProposalCreatedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMqProposalCreatedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare("proposal_exchange", ExchangeType.Fanout, durable: true);
        _channel.QueueDeclare("proposal_created_queue", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind("proposal_created_queue", "proposal_exchange", "");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var proposalEvent = JsonSerializer.Deserialize<ProposalCreatedEvent>(json);

            if (proposalEvent != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<ProposalCreatedEventHandler>();
                await handler.HandleAsync(proposalEvent);
            }
        };

        _channel.BasicConsume("proposal_created_queue", autoAck: true, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
