using AutoMapper;
using Bamboo.Models;
using Bamboo.DTO;
using System.Text.Json;

namespace Bamboo.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<AddCartDto, Cart>()
                .ForMember(dest => dest.productsIdsAndQuantities,
                           opt => opt.MapFrom(src => JsonSerializer.Serialize(src.productsIdsAndQuantities, new JsonSerializerOptions())));

            CreateMap<EditCartDto, Cart>()
                .ForMember(dest => dest.productsIdsAndQuantities,
                           opt => opt.MapFrom(src => JsonSerializer.Serialize(src.productsIdsAndQuantities, new JsonSerializerOptions())));

            CreateMap<Cart, ReadCartDto>()
                .ForMember(dest => dest.productsIdsAndQuantities,
                           opt => opt.MapFrom(src => src.productsIdsAndQuantities));
        }
    }
}
