namespace Shared.Services.IServices;
public interface IProducerBusService
{
    Task<bool> Publish<T1, T2>(T1 key, T2 value, string topic);
    Task CreateTopic();
}
