using AutoMapper;
using ShoppingWebsiteAPI.Models;

namespace ShoppingWebsiteAPI.DependencyInjections
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid())
                );
        }
    }
}
