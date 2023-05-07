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

            CreateMap<Product, ReadProductDto>().ForMember(ReadProductDto => ReadProductDto.businessDto,
                opt => opt.MapFrom(product => product.business));

            CreateMap<EditProductDto, Product>();
        }
    }
}
