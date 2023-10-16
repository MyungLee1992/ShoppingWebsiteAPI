using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByIdAsync(Guid id);
    }
}
