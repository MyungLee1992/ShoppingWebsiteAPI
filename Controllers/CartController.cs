using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;
using ShoppingWebsiteAPI.Services;

namespace ShoppingWebsiteAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public CartController(DataContext context, IUserService userService) {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CartItem>>> GetCartItems() {
            var user = await _context.Users
                .Where(user => user.UserName == _userService.GetMyName())
                .FirstOrDefaultAsync();

            if (user == null) {
                return BadRequest("Please log in to proceed.");
            }

            var cart = await _context.Carts
                .Where(cart => cart.User == user)
                .FirstAsync();

            var cartItems = await _context.CartItems
                .Where(cartItem => cartItem.Cart == cart)
                .Select(x => new {
                    id = x.Id,
                    cart = new CartDto {
                        Id = x.Cart.Id,
                        User = new UserDto {
                            UserName = _userService.GetMyName(),
                        }
                    },
                    item = x.Item,
                    quantity = x.Quantity,
                    price = x.Price
                })
                .ToListAsync();

            return Ok(cartItems);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CartItem>> AddCartItem(Item item) {
            var user = await _context.Users
                .Where(user => user.UserName == _userService.GetMyName())
                .FirstOrDefaultAsync();

            var cart = await _context.Carts
                .Where(cart => cart.User == user)
                .FirstAsync();

            var cartItem = await _context.CartItems
                .Where(cartItem => cartItem.Cart == cart && cartItem.Item == item)
                .FirstOrDefaultAsync();

            if (cartItem == null) {
                cartItem = new CartItem {
                    Cart = cart,
                    Item = item,
                    Price = item.Price
                };
            } else {
                cartItem.Quantity++;
                cartItem.Price = (item.Price * cartItem.Quantity);
            }
                
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return Ok(cartItem);
        }

        [HttpPut("update")]
        public async Task<ActionResult<CartItem>> UpdateCartItem(CartItem cartItem) {
            var existingCartItem = await _context.CartItems.FindAsync(cartItem.Id);
            if (existingCartItem == null) {
                return BadRequest("Cart Item not found.");
            }

            existingCartItem.Quantity = cartItem.Quantity;
            existingCartItem.Price = cartItem.Price;

            _context.CartItems.Update(existingCartItem);
            await _context.SaveChangesAsync();

            return Ok(existingCartItem);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Cart>> DeleteCartItemByCartId(int id) {
            var existingCart = await _context.Carts.FindAsync(id);
            if (existingCart == null) {
                return BadRequest("Cart not found.");
            }

            _context.Carts.Remove(existingCart);
            await _context.SaveChangesAsync();

            return Ok(existingCart);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteCart() {
            var existingCart = await _context.Carts
                .Include(cart => cart.User)
                .ThenInclude(user => user.UserName == _userService.GetMyName())
                .FirstOrDefaultAsync();

            if (existingCart == null) {
                return BadRequest("Cart not found.");
            }

            _context.Carts.Remove(existingCart);
            await _context.SaveChangesAsync();

            return Ok(existingCart);
        }
    }
}
