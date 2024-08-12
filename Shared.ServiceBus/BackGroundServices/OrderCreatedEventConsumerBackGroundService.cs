namespace Shared.Services.BackGroundServices;

public class OrderCreatedEventConsumerBackGroundService(IConsumerBusService _consumerService) : BackgroundService
{
    protected override  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = _consumerService.GetConsumer<string,OrderCreatedEvent>(KafkaTopicConstants.OrderCretedTopic);
 
        consumer.Subscribe(KafkaTopicConstants.OrderCretedTopic);
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(5000);
            if(consumeResult != null)
            {
                try
                {
                    var totalPrice = consumeResult.Message.Value.TotalPrice;
                    var ordeCode = consumeResult.Message.Value.OrderCode;
                    var userId = consumeResult.Message.Value.UserId;
                    Console.WriteLine($"TotalPrice: {totalPrice}, orderCode: {ordeCode}, UserId: {userId}");
                    ///decrease from stock
                    consumer.Commit(consumeResult);
                }
                catch (Exception ex)
                {
                   Console.WriteLine(JsonSerializer.Serialize(ex));
                }
      
            }
        }
        return Task.CompletedTask;
    }
}
