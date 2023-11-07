using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(DataContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsByCartAsync(Guid cartId) 
        {
            return await FindByCondition(cartItem => cartItem.Cart.Id == cartId)
                .Include(cartItem => cartItem.Cart)
                .ThenInclude(cart => cart.User)
                .Include(cartItem => cartItem.Item)
                .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemByIdAsync(Guid id)
        {
            return await FindByCondition(cartItem => cartItem.Id == id)
                .Include(cartItem => cartItem.Cart)
                .ThenInclude(cart => cart.User)
                .Include(cartItem => cartItem.Item)
                .FirstOrDefaultAsync();
        }

        public async Task<CartItem?> GetCartItemByCartAndItemAsync(Guid cartId, Guid itemId)
        {
            return await FindByCondition(cartItem => cartItem.Cart.Id == cartId && cartItem.Item.Id == itemId)
                .Include(cartItem => cartItem.Cart)
                .ThenInclude(cart => cart.User)
                .Include(cartItem => cartItem.Item)
                .FirstOrDefaultAsync();
        }

        public void AddCartItem(CartItem cartItem)
        {
            Add(cartItem);
        }

        public void UpdateCartItem(CartItem cartItem)
        {
            Update(cartItem);
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            Remove(cartItem);
        }
    }
}
