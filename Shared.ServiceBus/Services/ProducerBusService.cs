namespace Shared.Services.Services;

public class ProducerBusService: IProducerBusService
{
    private  readonly ProducerConfig _config;
    private readonly string _bootstrapServers;
    public ProducerBusService(IConfiguration configuration)
    {
        _bootstrapServers = configuration?.GetSection("BusSettings")?.GetSection("Kafka")["BootstrapServers"] ?? "localhost:9094";
        _config = new ProducerConfig()
        {
            BootstrapServers = _bootstrapServers,
            Acks = Acks.All,
            MessageTimeoutMs =6000,
            //MessageSendMaxRetries = 6,
            //RetryBackoffMaxMs = 2000,
            //RetryBackoffMs = 2000,
            AllowAutoCreateTopics =true,
        };
    }

    public async Task<bool> Publish<T1,T2>(T1 key, T2 value,string topic)
    {
        using var producer = new ProducerBuilder<T1, T2>(_config)
            .SetKeySerializer(new CustomKafkaSerializer<T1>())
            .SetValueSerializer(new CustomKafkaSerializer<T2>()).Build();
        var message = new Message<T1, T2>()
        {
            Key = key,
            Value = value,
        };
        var result =await producer.ProduceAsync(topic,message);
        return result.Status == PersistenceStatus.Persisted;
    }
    public async Task CreateTopic()
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig()
        {
            BootstrapServers = _bootstrapServers,

        }).Build();
        var topics = new List<string> { KafkaTopicConstants.OrderCretedTopic };
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(20));
        var topicsMetadata = metadata.Topics.Select(x => x.Topic);
        var topicNames = topics.Where(a => !topicsMetadata.Any(s => s ==a)).ToList();
        var topicSpecifications = topicNames.Select(x => new TopicSpecification
        {
            Name = x,
            NumPartitions = 3,
            ReplicationFactor =1

        }).ToList();
        if(topicSpecifications?.Count() > 0)
        {
            await adminClient.CreateTopicsAsync(topicSpecifications);
        }
    }
   
}
