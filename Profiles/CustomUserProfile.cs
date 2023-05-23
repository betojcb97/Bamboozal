using AutoMapper;
using Bamboo.Profiles;
using Bamboo.Models;
using Bamboo.DTO;

namespace Bamboo.Profiles
{
    public class CustomUserProfile : Profile
    {
        public CustomUserProfile() 
        {
            CreateMap<AddCustomUserDto, CustomUser>();

            CreateMap<CustomUser, ReadCustomUserDto>()
                .ForMember(userDto => userDto.addressDto,
                opt => opt.MapFrom(user => user.address))
                .ForMember(userDto => userDto.businessDto,
                opt => opt.MapFrom(user => user.business));

            CreateMap<EditCustomUserDto, CustomUser>();

        }
    }
}
