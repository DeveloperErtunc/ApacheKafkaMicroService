namespace Shared.Services.Services;

public class OrderService(IProducerBusService _busService) : IOrderService
{
    public async Task<bool> Create(OrderCreateRequestDTO request)
    {
        var ordeCode = Guid.NewGuid().ToString();
        //saved to database order
        var orderCreatedEvent = new OrderCreatedEvent(ordeCode, request.UserId, request.TotalPrice);

        return  await  _busService.Publish(ordeCode, orderCreatedEvent, KafkaTopicConstants.OrderCretedTopic);
    }
}
