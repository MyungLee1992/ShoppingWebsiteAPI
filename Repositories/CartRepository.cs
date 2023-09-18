using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(DataContext dbContext) : base(dbContext) { }

        public async Task<Cart?> GetCartByIdAsync(Guid id)
        {
            return await FindByCondition(cart => cart.Id == id).FirstOrDefaultAsync();
        }
    }
}
