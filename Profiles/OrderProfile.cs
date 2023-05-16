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
            CreateMap<AddOrderDto, Order>()
                .ForMember(dest => dest.products, opt => opt.Ignore())
                .ForMember(dest => dest.productsIds, opt => opt.MapFrom(src => src.productsIds));

            CreateMap<Order, ReadOrderDto>()
                .ForMember(dest => dest.productsIds, opt => opt.MapFrom(src => src.products.Select(p => p.productID)))
                .ForMember(dest => dest.productsDto, opt => opt.MapFrom(src => src.products))
                .ForMember(dest => dest.userDto, opt => opt.MapFrom(src => src.user))
                .ForMember(dest => dest.businessDto, opt => opt.MapFrom(src => src.business));

            CreateMap<EditOrderDto, Order>()
                .ForMember(dest => dest.products, opt => opt.Ignore())
                .ForMember(dest => dest.productsIds, opt => opt.MapFrom(src => src.productsIds));

        }
    }
}


