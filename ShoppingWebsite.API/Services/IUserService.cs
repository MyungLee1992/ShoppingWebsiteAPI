using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.Services
{
    public interface IUserService
    {
        string GetMyName();
        Task<UserDto?> GetUserByUserNameAsync(string userName);
    }
}
