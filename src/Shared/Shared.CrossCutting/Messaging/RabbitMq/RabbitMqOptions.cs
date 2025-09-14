namespace Shared.CrossCutting.Messaging.RabbitMq;

public class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string Exchange { get; set; } = "insurance.exchange";
    public bool Durable { get; set; } = true;
    public ushort PrefetchCount { get; set; } = 10;

    public QueuesOptions Queues { get; set; } = new();
}

public class QueuesOptions
{
    public string ProposalCreated { get; set; } = "proposal-created";
}
