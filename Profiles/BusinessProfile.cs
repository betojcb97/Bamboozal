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
        }
    }
}
