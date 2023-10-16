using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartItem>>> GetCartItems()
        {
            var cartItems = await _cartService.GetCartItemsAsync();
            if (cartItems == null)
            {
                BadRequest("Please log in to proceed.");
            }

            return Ok(cartItems);
        }

        [HttpPost("add/{itemId}")]
        public async Task<ActionResult> AddCartItem(Guid itemId, ItemDto itemDto)
        {
            await _cartService.CreateCartItemAsync(itemId, itemDto);
            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateCartItem(Guid id, CartItemDto cartItemDto)
        {
            var updated = await _cartService.UpdateCartItemAsync(id, cartItemDto);

            return updated ? Ok() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCartItem(Guid id)
        {
            var deleted = await _cartService.DeleteCartItemAsync(id);

            return deleted ? Ok() : NotFound();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteCart()
        {
            await _cartService.DeleteCart();

            return Ok();
        }
    }
}
