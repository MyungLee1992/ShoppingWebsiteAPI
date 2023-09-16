using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(Guid id);
        void AddItem(Item item);
        void UpdateItem(Item item);
        void RemoveItem(Item item);
    }
}
