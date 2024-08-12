namespace Shared.Services.Services;

public class ConsumerBusService : IConsumerBusService
{
    private readonly string _bootstrapServers;
    public ConsumerBusService(IConfiguration configuration)
    {
        _bootstrapServers = configuration?.GetSection("BusSettings")?.GetSection("Kafka")["BootstrapServers"] ?? "localhost:9094";
    }
    public IConsumer<T1, T2> GetConsumer<T1, T2>(string groupId)
    {
        var config = GetConsumerConfig(groupId);
        var consumer = new ConsumerBuilder<T1, T2>
         (config).SetKeyDeserializer(new CustomKafkaDeSerializer<T1>())
         .SetValueDeserializer(new CustomKafkaDeSerializer<T2>()).Build();
        return consumer;
    }
    public ConsumerConfig GetConsumerConfig(string groupId)
    {

        return new ConsumerConfig()
        {
            BootstrapServers = _bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
    }
}
