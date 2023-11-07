using AutoMapper;
using ShoppingWebsiteAPI.Models;
using ShoppingWebsiteAPI.Repositories;

namespace ShoppingWebsiteAPI.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IEnumerable<CartItemDto?>> GetCartItemsAsync()
        {
            var user = await _unitOfWork.Users.GetUserByUserNameAsync(_userService.GetMyName());
            if (user == null)
            {
                return null;
            }

            var cartItems = await _unitOfWork.CartItems.GetAllCartItemsByCartAsync(user.Cart.Id);

            return cartItems.Select(cartItem => _mapper.Map<CartItemDto>(cartItem));
        }

        public async Task<bool> CreateCartItemAsync(ItemDto itemDto) 
        {
            var user = await _unitOfWork.Users.GetUserByUserNameAsync(_userService.GetMyName()).ConfigureAwait(false);
            if (user == null)
            {
                return false;
            }

            var item = await _unitOfWork.Items.GetItemByIdAsync(itemDto.Id);
            if (item == null)
            {
                return false;
            }

            var cartItem = await _unitOfWork.CartItems.GetCartItemByCartAndItemAsync(user.Cart.Id, item.Id);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Cart = user.Cart,
                    Item = item,
                    Price = item.Price
                };
            } 
            else
            {
                cartItem = cartItem with
                {
                    Quantity = cartItem.Quantity + 1,
                    Price = (item.Price * cartItem.Quantity)
                };
            }

            _unitOfWork.CartItems.Update(cartItem);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateCartItemAsync(CartItemDto cartItemDto) 
        {
            var cartItem = await _unitOfWork.CartItems.GetCartItemByIdAsync(cartItemDto.Id);
            if (cartItem == null)
            {
                return false;
            }

            cartItem = cartItem with
            {
                Quantity = cartItemDto.Quantity,
                Price = cartItemDto.Price
            };
            
            _unitOfWork.CartItems.Update(cartItem);

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteCartItemAsync(Guid id) 
        {
            var cartItem = await _unitOfWork.CartItems.GetCartItemByIdAsync(id);
            if (cartItem == null)
            {
                return false;
            }

            _unitOfWork.CartItems.Remove(cartItem);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteCart()
        {
            var cart = await _unitOfWork.Carts
                .FindByCondition(cart => cart.User.UserName == _userService.GetMyName())
                .FirstOrDefaultAsync();

            if (cart != null)
            {
                var cartItems = await _unitOfWork.CartItems.GetAllCartItemsByCartAsync(cart.Id);
                foreach (var cartItem in cartItems)
                {
                    _unitOfWork.CartItems.Remove(cartItem);
                }

                await _unitOfWork.SaveAsync();
            }

            return true;
        }
    }
}
