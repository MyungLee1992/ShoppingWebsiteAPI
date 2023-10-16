using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(DataContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Item?> GetItemByIdAsync(Guid id)
        {
            return await FindByCondition(item => item.Id == id).FirstOrDefaultAsync();
        }

        public void AddItem(Item item)
        {
            Add(item);
        }

        public void UpdateItem(Item item)
        {
            Update(item);
        }

        public void RemoveItem(Item item)
        {
            Remove(item);
        }


    }
}
