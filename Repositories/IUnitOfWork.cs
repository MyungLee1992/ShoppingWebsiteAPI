namespace ShoppingWebsiteAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository Items { get; }
        Task SaveAsync();
    }
}
