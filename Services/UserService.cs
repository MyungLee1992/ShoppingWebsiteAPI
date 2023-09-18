using AutoMapper;
using Azure.Core;
using ShoppingWebsiteAPI.Data;
using ShoppingWebsiteAPI.Models;
using ShoppingWebsiteAPI.Repositories;
using System.Security.Claims;

namespace ShoppingWebsiteAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return result;
        }
        public async Task<UserDto?> GetUserByUserNameAsync(string userName)
        {
            var user = await _unitOfWork.Users.GetUserByUserNameAsync(this.GetMyName());

            return _mapper.Map<UserDto>(user);
        }
    }
}
