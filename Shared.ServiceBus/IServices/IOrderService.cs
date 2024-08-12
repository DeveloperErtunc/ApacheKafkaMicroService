namespace Shared.Services.IServices;

public interface IOrderService
{
    Task<bool> Create(OrderCreateRequestDTO request);
}
