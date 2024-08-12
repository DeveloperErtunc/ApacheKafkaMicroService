namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IOrderService _orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateRequestDTO request) => Ok(await _orderService.Create(request)) ;
}
