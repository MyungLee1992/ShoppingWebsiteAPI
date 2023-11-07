using AutoMapper;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.DependencyInjections
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<ItemDto, Item>().ReverseMap(); 
            CreateMap<CartDto, Cart>().ReverseMap();
            CreateMap<CartItemDto, CartItem>().ReverseMap();
        }
    }
}
