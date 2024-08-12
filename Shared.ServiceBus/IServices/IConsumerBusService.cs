namespace Shared.Services.Services;

public interface IConsumerBusService
{
    ConsumerConfig GetConsumerConfig(string groupId);
    IConsumer<T1, T2> GetConsumer<T1, T2>(string groupId);
}