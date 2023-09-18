using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public CartController(DataContext context, IUserService userService, ICartService cartService)
        {
            _context = context;
            _userService = userService;
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
        public async Task<ActionResult<CartItem>> AddCartItem(Guid itemId, ItemDto itemDto)
        {
            await _cartService.CreateCartItemAsync(itemId, itemDto);
            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<CartItem>> UpdateCartItem(Guid id, CartItemDto cartItemDto)
        {
            var updated = await _cartService.UpdateCartItemAsync(id, cartItemDto);

            return updated ? Ok() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<CartItem>> DeleteCartItem(Guid id)
        {
            var existingCart = await _context.Carts.FindAsync(id);
            if (existingCart == null)
            {
                return BadRequest("Cart not found.");
            }

            _context.Carts.Remove(existingCart);
            await _context.SaveChangesAsync();

            return Ok(existingCart);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteCart()
        {
            await _cartService.DeleteCart();

            return Ok();
        }
    }
}
