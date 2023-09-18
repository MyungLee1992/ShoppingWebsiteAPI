using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Repositories
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetAllCartItemsAsync();
        Task<IEnumerable<CartItem>> GetAllCartItemsByCartAsync(Guid cartId);
        Task<CartItem?> GetCartItemByIdAsync(Guid id);
        Task<CartItem?> GetCartItemByCartAndItemAsync(Guid cartId, Guid itemId);
        void AddCartItem(CartItem cartItem);
        void UpdateCartItem(CartItem cartItem);
        void RemoveCartItem(CartItem cartItem);
    }
}
