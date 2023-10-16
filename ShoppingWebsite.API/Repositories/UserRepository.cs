using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await FindByCondition(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await FindByCondition(user => user.UserName == userName)
                .Include(user => user.Cart)
                .FirstOrDefaultAsync();
        }
    }
}
