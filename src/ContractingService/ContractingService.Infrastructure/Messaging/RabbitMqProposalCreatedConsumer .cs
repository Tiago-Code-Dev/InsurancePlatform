namespace ContractingService.Infrastructure.Messaging;

using ContractingService.Application.EventHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.CrossCutting.Messaging.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqProposalCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMqProposalCreatedConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var hostName = _configuration["RabbitMq:HostName"] ?? "localhost";
        var userName = _configuration["RabbitMq:UserName"] ?? "guest";
        var password = _configuration["RabbitMq:Password"] ?? "guest";

        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            DispatchConsumersAsync = true
        };

        const int maxRetries = 10;
        int retryCount = 0;

        while (retryCount < maxRetries)
        {
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare("proposal_exchange", ExchangeType.Fanout, durable: true);
                _channel.QueueDeclare("proposal_created_queue", durable: true, exclusive: false, autoDelete: false);
                _channel.QueueBind("proposal_created_queue", "proposal_exchange", "");

                Console.WriteLine("✅ Conectado ao RabbitMQ com sucesso.");
                return;
            }
            catch (Exception ex)
            {
                retryCount++;
                Console.WriteLine($"⏳ Tentativa {retryCount}/{maxRetries} falhou: {ex.Message}");
                Thread.Sleep(3000);
            }
        }

        throw new Exception("❌ Não foi possível conectar ao RabbitMQ após múltiplas tentativas.");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var proposalEvent = JsonSerializer.Deserialize<ProposalCreatedEvent>(json);

                if (proposalEvent is not null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<ProposalCreatedEventHandler>();
                    await handler.HandleAsync(proposalEvent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
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
