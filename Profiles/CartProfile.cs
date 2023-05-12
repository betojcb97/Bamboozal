using AutoMapper;
using Bamboo.Profiles;
using Bamboo.Models;
using Bamboo.DTO;

namespace Bamboo.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<AddCartDto, Cart>()
                .ForMember(dest => dest.products, opt => opt.Ignore())
                .ForMember(dest => dest.productsIds, opt => opt.MapFrom(src => src.productsIds));

            CreateMap<Cart, ReadCartDto>()
                .ForMember(dest => dest.productsIds, opt => opt.MapFrom(src => src.products.Select(p => p.productID)))
                .ForMember(dest => dest.productsDto, opt => opt.MapFrom(src => src.products));

            CreateMap<EditCartDto, Cart>()
                .ForMember(dest => dest.products, opt => opt.Ignore())
                .ForMember(dest => dest.productsIds, opt => opt.MapFrom(src => src.productsIds));
        }
    }
}


