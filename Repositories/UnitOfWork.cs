using ShoppingWebsiteAPI.Data;

namespace ShoppingWebsiteAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed;
        private readonly DataContext _dbContext;
        public IItemRepository Items { get; }

        public UnitOfWork(DataContext dbContext, IItemRepository itemRepository)
        {
            _dbContext = dbContext;
            Items = itemRepository;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (! disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
