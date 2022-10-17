using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopOnline.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductDTO>>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = await cartRepository.GetCartProductsAsync(cartItems);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductDTO>>>> StoreCartItems(List<CartItem> cartItems)
        {
            var result = await cartRepository.StoreCartItemsAsync(cartItems);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
        {
            var result = await cartRepository.AddToCartAsync(cartItem);
            return Ok(result);
        }

        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem)
        {
            var result = await cartRepository.UpdateQuantityAsync(cartItem);
            return Ok(result);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId)
        {
            var result = await cartRepository.RemoveItemFromCartAsync(productId);
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
        {
            return await cartRepository.GetCartItemCountAsync();
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartProductDTO>>>> GetDbCartProducts()
        {
            var result = await cartRepository.GetDbCartProductsAsync();
            return Ok(result);
        }
    }
}
