using AutoMapper;
using Bamboo.Profiles;
using Bamboo.Models;
using Bamboo.DTO;

namespace Bamboo.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddOrderDto, Order>();

            CreateMap<Order, ReadOrderDto>()
                .ForMember(dest => dest.userDto, opt => opt.MapFrom(src => src.user));

            CreateMap<EditOrderDto, Order>();
        }
    }
}


