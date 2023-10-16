using ShoppingWebsiteAPI.Models;
using System.Collections.Generic;

namespace ShoppingWebsiteAPI.Services
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetItemsAsync();
        Task<ItemDto?> GetItemAsync(Guid id);
        Task<bool> CreateItemAsync(ItemDto itemDto);
        Task<bool> UpdateItemAsync(Guid id, ItemDto itemDto);
        Task<bool> DeleteItemAsync(Guid id);
    }
}
