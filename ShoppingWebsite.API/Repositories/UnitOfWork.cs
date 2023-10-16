using ShoppingWebsiteAPI.Data;

namespace ShoppingWebsiteAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed;
        private readonly DataContext _dbContext;
        public IUserRepository Users { get; }
        public IItemRepository Items { get; }
        public ICartRepository Carts { get; }
        public ICartItemRepository CartItems { get; }

        public UnitOfWork
        (
            DataContext dbContext, 
            IUserRepository userRepository,
            IItemRepository itemRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository
        )
        {
            _dbContext = dbContext;
            Users = userRepository;
            Items = itemRepository;
            Carts = cartRepository;
            CartItems = cartItemRepository;
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
