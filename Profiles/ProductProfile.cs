using AutoMapper;
using Bamboo.Profiles;
using Bamboo.Models;
using Bamboo.DTO;

namespace Bamboo.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductDto, Product>();

            CreateMap<Product, ReadProductDto>().ForMember(dto => dto.businessDto,
                opt => opt.MapFrom(product => product.business));

            CreateMap<EditProductDto, Product>();
        }
    }
}

