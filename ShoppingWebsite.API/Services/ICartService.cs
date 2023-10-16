using ShoppingWebsiteAPI.Models;
using System.Collections.Generic;

namespace ShoppingWebsiteAPI.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemDto?>> GetCartItemsAsync();
        Task<bool> CreateCartItemAsync(Guid itemId, ItemDto itemDto);
        Task<bool> UpdateCartItemAsync(Guid id, CartItemDto itemDto);
        Task<bool> DeleteCartItemAsync(Guid id);
        Task<bool> DeleteCart();
    }
}
