using AutoMapper;
using Bamboo.Profiles;
using Bamboo.Models;
using Bamboo.DTO;

namespace Bamboo.Profiles
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile() 
        {
            CreateMap<AddBusinessDto, Business>();

            CreateMap<Business, ReadBusinessDto>().ForMember(businessDto => businessDto.addressDto,
                opt => opt.MapFrom(business => business.address));

            CreateMap<EditBusinessDto, Business>();

        }
    }
}
