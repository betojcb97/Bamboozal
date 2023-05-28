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
            CreateMap<AddCartDto, Cart>();

            CreateMap<Cart, ReadCartDto>();

            CreateMap<EditCartDto, Cart>();
        }
    }
}


