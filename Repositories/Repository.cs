using ShoppingWebsiteAPI.Data;
using System.Linq.Expressions;

namespace ShoppingWebsiteAPI.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _dbContext;

        protected Repository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> FindAll()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}
