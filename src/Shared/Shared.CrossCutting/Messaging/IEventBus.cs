namespace Shared.CrossCutting.Messaging;

public interface IEventBus
{
    Task PublishAsync<T>(T @event, string queueName) where T : class;
    Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class; // usaremos no Contracting depois
}
