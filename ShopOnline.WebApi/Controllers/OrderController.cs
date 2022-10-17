using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopOnline.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderDTO>>>> GetOrders()
        {
            var result = await orderRepository.GetOrdersAsync();
            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<List<OrderDetailsDTO>>>> GetOrderDetails(int orderId)
        {
            var result = await orderRepository.GetOrderDetailsAsync(orderId);
            return Ok(result);
        }
    }
}
