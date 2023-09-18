namespace ShoppingWebsiteAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IItemRepository Items { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }
        Task SaveAsync();
    }
}
