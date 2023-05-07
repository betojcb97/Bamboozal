using AutoMapper;
using Bamboo.Profiles;
using Bamboo.Models;
using Bamboo.DTO;

namespace Bamboo.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile() 
        {
            CreateMap<AddAddressDto, Address>();

            CreateMap<Address, ReadAddressDto>();

            CreateMap<EditAddressDto,Address>();
        }
    }
}
